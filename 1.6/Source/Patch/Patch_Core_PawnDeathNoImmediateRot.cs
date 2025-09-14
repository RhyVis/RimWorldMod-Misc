namespace Rhynia.Misc.Patch;

public class Patch_Core_PawnDeathNoImmediateRot
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (AccessTools.Method(typeof(Pawn), nameof(Pawn.Kill)) is { } method)
            {
                harmony.Patch(
                    method,
                    transpiler: new(typeof(Patch_Core_PawnDeathNoImmediateRot), nameof(Transpiler))
                );
                Info("Applied patch Core_PawnDeathNoImmediateRot");
            }
            else
                Error("Failed to find method Pawn.Kill");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_PawnDeathNoImmediateRot: {ex}");
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);

        // First pattern - ToxicBuildup
        matcher.MatchStartForward(
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldfld, AccessTools.Field(typeof(Pawn), nameof(Pawn.health))),
            new(
                OpCodes.Ldfld,
                AccessTools.Field(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.hediffSet))
            ),
            new(
                OpCodes.Ldsfld,
                AccessTools.Field(typeof(HediffDefOf), nameof(HediffDefOf.ToxicBuildup))
            ),
            new(OpCodes.Ldc_I4_0),
            new(
                OpCodes.Callvirt,
                AccessTools.Method(
                    typeof(HediffSet),
                    nameof(HediffSet.GetFirstHediffOfDef),
                    [typeof(HediffDef), typeof(bool)]
                )
            ),
            new(OpCodes.Stloc_S)
        );

        if (matcher.IsValid)
        {
            var loc = matcher.InstructionAt(6).Clone().operand;
            matcher.RemoveInstructions(7);
            matcher.Insert(new(OpCodes.Ldnull), new(OpCodes.Stloc_S, loc));
        }
        else
        {
            Error("Failed to match ToxicBuildup def in Pawn.Kill");
            throw new InvalidOperationException(
                "Transpiler failed: ToxicBuildup pattern not found"
            );
        }

        // Reset matcher to start for second pattern
        matcher.Start();

        // Second pattern - Scaria
        matcher.MatchStartForward(
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldfld, AccessTools.Field(typeof(Pawn), nameof(Pawn.health))),
            new(
                OpCodes.Ldfld,
                AccessTools.Field(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.hediffSet))
            ),
            new(OpCodes.Ldsfld, AccessTools.Field(typeof(HediffDefOf), nameof(HediffDefOf.Scaria))),
            new(OpCodes.Ldc_I4_0),
            new(
                OpCodes.Callvirt,
                AccessTools.Method(
                    typeof(HediffSet),
                    nameof(HediffSet.GetFirstHediffOfDef),
                    [typeof(HediffDef), typeof(bool)]
                )
            ),
            new(OpCodes.Stloc_S)
        );

        if (matcher.IsValid)
        {
            var loc = matcher.InstructionAt(6).Clone().operand;
            matcher.RemoveInstructions(7);
            matcher.Insert(new(OpCodes.Ldnull), new(OpCodes.Stloc_S, loc));
        }
        else
        {
            Error("Failed to match Scaria def in Pawn.Kill");
            throw new InvalidOperationException("Transpiler failed: Scaria pattern not found");
        }

        return matcher.InstructionEnumeration();
    }
}
