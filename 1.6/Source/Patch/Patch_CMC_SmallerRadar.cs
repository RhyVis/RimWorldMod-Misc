namespace Rhynia.Misc.Patch;

public static class Patch_CMC_SmallerFacility
{
    public static void Apply(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("TOT_DLL_test.Building_AESARadar");
            if (type is null)
            {
                Out.Error("TOT_DLL_test.Building_AESARadar not found.");
                return;
            }
            var method = AccessTools.Method(type, "DrawAt");
            if (method is null)
            {
                Out.Error("TOT_DLL_test.Building_AESARadar.DrawAt not found.");
                return;
            }
            Out.Debug($"Render size will be replaced with {Vec}");
            harmony.Patch(
                original: method,
                transpiler: new(typeof(Patch_CMC_SmallerFacility), nameof(Transpiler))
            );
            Out.Info("Applied Patch_CMC_SmallerRadar successfully.");
        }
        catch (Exception ex)
        {
            Out.Error($"Failed to apply Patch_CMC_SmallerRadar: {ex}");
        }
    }

    public static readonly Vector3 Vec = new(5f, 1f, 5f);

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var type = AccessTools.TypeByName("TOT_DLL_test.Building_AESARadar");
        var originalVecField = AccessTools.Field(type, "vec");
        var customVecField = AccessTools.Field(typeof(Patch_CMC_SmallerFacility), nameof(Vec));

        bool patched = false;

        foreach (var instruction in instructions)
            if (
                !patched
                && instruction.opcode == OpCodes.Ldsfld
                && instruction.operand != null
                && instruction.operand.Equals(originalVecField)
            )
            {
                yield return new CodeInstruction(OpCodes.Ldsfld, customVecField);
                Log.Message("Patched Building_AESARadar.vec to new Vec");
                patched = true;
            }
            else
                yield return instruction;
    }
}
