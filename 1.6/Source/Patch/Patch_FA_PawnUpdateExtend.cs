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
        var originalGetter = AccessTools.PropertyGetter(typeof(Pawn), nameof(Pawn.IsColonist));
        var replaceMethod = AccessTools.Method(
            typeof(Patch_FA_PawnUpdateExtend_Helper),
            nameof(Patch_FA_PawnUpdateExtend_Helper.Is)
        );

        foreach (var code in instructions)
            if (code.opcode == OpCodes.Callvirt && code.operand as MethodInfo == originalGetter)
                yield return new CodeInstruction(OpCodes.Call, replaceMethod);
            else
                yield return code;
    }
}

public static class Patch_FA_PawnUpdateExtend_Helper
{
    public static bool Is(Pawn pawn) =>
        pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony;
}
