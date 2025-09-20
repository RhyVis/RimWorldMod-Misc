namespace Rhynia.Misc.Patch;

internal static class Patch_FA_PawnUpdateExtend
{
    static bool IsValid(Pawn pawn) => pawn.IsColonist || pawn.IsPrisoner || pawn.IsSlave;

    internal static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    ) =>
        instructions
            .AsCodeMatcher()
            .MatchStartForward(
                CodeMatch.Calls(AccessTools.PropertyGetter(typeof(Pawn), nameof(Pawn.IsColonist)))
            )
            .ThrowIfInvalid(
                "Could not find target instruction to patch in FacialAnimation.FacialAnimationControllerComp.CheckUpdatable"
            )
            .SetInstruction(CodeInstruction.Call(() => IsValid(default)))
            .InstructionEnumeration();
}
