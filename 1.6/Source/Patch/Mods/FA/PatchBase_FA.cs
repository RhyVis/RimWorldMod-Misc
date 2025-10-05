namespace Rhynia.Misc.Patch;

internal class PatchBase_FA : PatchBase
{
    public override string Name => "Facial Animation";
    public override string ModId => "Nals.FacialAnimation";
    public override string LogLabel => "Rhynia.Misc";

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "FA_PawnUpdateExtend",
                () => SettingsProxy.GetBool("FA_PawnUpdateExtend"),
                () =>
                    AccessTools.Method(
                        AccessTools.TypeByName("FacialAnimation.FacialAnimationControllerComp"),
                        "CheckUpdatable",
                        [typeof(int)]
                    ),
                () =>
                    new(
                        typeof(Patch_FA_PawnUpdateExtend),
                        nameof(Patch_FA_PawnUpdateExtend.Transpiler)
                    ),
                HarmonyPatchType.Transpiler
            ),
        ];
}
