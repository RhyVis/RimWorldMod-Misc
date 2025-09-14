namespace Rhynia.Misc.Patch;

internal static class Patch_FA_PawnUpdateExtend
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("FacialAnimation.FacialAnimationControllerComp");
            if (type is null)
            {
                Error("Facial Animation not found.");
                return;
            }

            var method = AccessTools.Method(type, "CheckUpdatable", [typeof(int)]);
            if (method is null)
            {
                Error("FacialAnimation.FacialAnimationControllerComp.CheckUpdatable not found.");
                return;
            }

            harmony.Patch(
                original: method,
                transpiler: new(typeof(Patch_FA_PawnUpdateExtend), nameof(Transpiler))
            );

            Info("Applied patch FA_PawnUpdateExtend.");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply FA_PawnUpdateExtend: {ex}");
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);

        matcher
            .MatchStartForward(
                new CodeMatch(
                    OpCodes.Callvirt,
                    AccessTools.PropertyGetter(typeof(Pawn), nameof(Pawn.IsColonist))
                )
            )
            .ThrowIfInvalid(
                "Could not find target instruction to patch in FacialAnimation.FacialAnimationControllerComp.CheckUpdatable"
            )
            .SetInstruction(
                new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.Method(
                        typeof(Patch_FA_PawnUpdateExtend_Helper),
                        nameof(Patch_FA_PawnUpdateExtend_Helper.IsValid)
                    )
                )
            );

        return matcher.InstructionEnumeration();
    }
}

public static class Patch_FA_PawnUpdateExtend_Helper
{
    public static bool IsValid(Pawn pawn) =>
        pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony;
}
