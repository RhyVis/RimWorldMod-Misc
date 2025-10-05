namespace Rhynia.Misc.Patch;

internal class PatchBase_CTA : PatchBase
{
    public override string Name => "Ideology";
    public override string ModId => "tot.celetech.mkiii";
    public override string LogLabel => "Rhynia.Misc";

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "CTA_SmallerFacility",
                () => SettingsProxy.GetBool("CTA_SmallerFacility"),
                () =>
                    AccessTools.Method(
                        AccessTools.TypeByName("TOT_DLL_test.Building_AESARadar"),
                        "DrawAt"
                    ),
                () =>
                    new(
                        typeof(Patch_CTA_SmallerFacility),
                        nameof(Patch_CTA_SmallerFacility.Transpiler)
                    ),
                HarmonyPatchType.Transpiler,
                true
            ),
        ];
}
