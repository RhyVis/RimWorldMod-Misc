namespace Rhynia.Misc.Patch;

internal class PatchBase_Addon(Harmony harmony) : PatchBase(harmony)
{
    public override string Name => "Rhynia Misc Tweaks Addon";
    public override string ModId => "Rhynia.Mod.Misc";
    public override string LogLabel => "Rhynia.Misc";

    protected override bool ShouldApply => true;

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "Addon_ScreamingIncident",
                () => SettingsProxy.GetBool("Addon_ScreamingIncident"),
                () =>
                    AccessTools.Method(
                        typeof(LetterStack),
                        nameof(LetterStack.ReceiveLetter),
                        [typeof(Letter), typeof(string), typeof(int), typeof(bool)]
                    ),
                () =>
                    new(
                        typeof(Patch_Addon_ScreamingIncident),
                        nameof(Patch_Addon_ScreamingIncident.Postfix)
                    ),
                HarmonyPatchType.Postfix
            ),
        ];
}
