namespace Rhynia.Misc.Patch;

public class Patch_Core_PawnDeathNoImmediateRot
{
    internal static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
    {
        var matcher = new CodeMatcher(instructions);

        var fPawnHealth = AccessTools.Field(typeof(Pawn), nameof(Pawn.health));
        var fHediffSet = AccessTools.Field(
            typeof(Pawn_HealthTracker),
            nameof(Pawn_HealthTracker.hediffSet)
        );
        var mGetFirstHediffOfDef = AccessTools.Method(
            typeof(HediffSet),
            nameof(HediffSet.GetFirstHediffOfDef),
            [typeof(HediffDef), typeof(bool)]
        );

        // First pattern - ToxicBuildup
        matcher
            .MatchStartForward(
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, fPawnHealth),
                new(OpCodes.Ldfld, fHediffSet),
                new(
                    OpCodes.Ldsfld,
                    AccessTools.Field(typeof(HediffDefOf), nameof(HediffDefOf.ToxicBuildup))
                ),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Callvirt, mGetFirstHediffOfDef),
                new(OpCodes.Stloc_S)
            )
            .ThrowIfInvalid("Failed to match start of ToxicBuildup pattern");

        var loc1 = matcher.InstructionAt(6).Clone().operand;
        matcher.RemoveInstructions(7);
        matcher.Insert(new(OpCodes.Ldnull), new(OpCodes.Stloc_S, loc1));

        matcher.Start();

        // Second pattern - Scaria
        matcher
            .MatchStartForward(
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, fPawnHealth),
                new(OpCodes.Ldfld, fHediffSet),
                new(
                    OpCodes.Ldsfld,
                    AccessTools.Field(typeof(HediffDefOf), nameof(HediffDefOf.Scaria))
                ),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Callvirt, mGetFirstHediffOfDef),
                new(OpCodes.Stloc_S)
            )
            .ThrowIfInvalid("Failed to match start of Scaria pattern");

        var loc2 = matcher.InstructionAt(6).Clone().operand;
        matcher.RemoveInstructions(7);
        matcher.Insert(new(OpCodes.Ldnull), new(OpCodes.Stloc_S, loc2));

        return matcher.InstructionEnumeration();
    }
}
