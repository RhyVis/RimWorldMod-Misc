namespace Rhynia.Misc.Patch;

internal class PatchBase_Ideology(Harmony harmony) : PatchBase(harmony)
{
    public override string Name => "Ideology";
    public override string ModId => "Ludeon.Rimworld.Ideology";
    public override string LogLabel => "Rhynia.Misc";

    protected override bool ShouldApply => ModsConfig.IdeologyActive;

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "Ideology_FastRelic",
                () => SettingsProxy.GetBool("Ideology_FastRelic"),
                () =>
                    AccessTools.Method(
                        typeof(QuestPart_SubquestGenerator),
                        nameof(QuestPart_SubquestGenerator.QuestPartTick)
                    ),
                () =>
                    new(typeof(Patch_Ideology_FastRelic), nameof(Patch_Ideology_FastRelic.Prefix)),
                HarmonyPatchType.Prefix
            ),
        ];
}
