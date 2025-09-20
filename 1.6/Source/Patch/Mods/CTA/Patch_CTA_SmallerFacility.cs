namespace Rhynia.Misc.Patch;

internal static class Patch_CTA_SmallerFacility
{
    static readonly Vector3 r = new(5f, 1f, 5f);

    internal static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    ) =>
        instructions
            .AsCodeMatcher()
            .MatchStartForward(
                CodeMatch.LoadsField(
                    AccessTools.Field(
                        AccessTools.TypeByName("TOT_DLL_test.Building_AESARadar"),
                        "vec"
                    )
                )
            )
            .ThrowIfInvalid(
                "Could not find target instruction to patch in TOT_DLL_test.Building_AESARadar.DrawAt"
            )
            .SetOperandAndAdvance(AccessTools.Field(typeof(Patch_CTA_SmallerFacility), nameof(r)))
            .InstructionEnumeration();
}
