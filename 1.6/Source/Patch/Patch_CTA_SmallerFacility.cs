namespace Rhynia.Misc.Patch;

internal static class Patch_CTA_SmallerFacility
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var type = AccessTools.TypeByName("TOT_DLL_test.Building_AESARadar");
            if (type is null)
            {
                Error("TOT_DLL_test.Building_AESARadar not found.");
                return;
            }

            var method = AccessTools.Method(type, "DrawAt");
            if (method is null)
            {
                Error("TOT_DLL_test.Building_AESARadar.DrawAt not found.");
                return;
            }

            Debug($"Render size will be replaced with {Patch_CTA_SmallerFacility_Helper.Vec}");

            harmony.Patch(
                original: method,
                transpiler: new(typeof(Patch_CTA_SmallerFacility), nameof(Transpiler))
            );

            Info("Applied patch CMC_SmallerFacility.");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch CMC_SmallerFacility: {ex}");
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);

        matcher
            .MatchStartForward(
                new CodeMatch(
                    OpCodes.Ldsfld,
                    AccessTools.Field(
                        AccessTools.TypeByName("TOT_DLL_test.Building_AESARadar"),
                        "vec"
                    )
                )
            )
            .ThrowIfInvalid(
                "Could not find target instruction to patch in TOT_DLL_test.Building_AESARadar.DrawAt"
            )
            .SetOperandAndAdvance(
                AccessTools.Field(
                    typeof(Patch_CTA_SmallerFacility_Helper),
                    nameof(Patch_CTA_SmallerFacility_Helper.Vec)
                )
            );

        return matcher.InstructionEnumeration();
    }
}

public static class Patch_CTA_SmallerFacility_Helper
{
    public static readonly Vector3 Vec = new(5f, 1f, 5f);
}
