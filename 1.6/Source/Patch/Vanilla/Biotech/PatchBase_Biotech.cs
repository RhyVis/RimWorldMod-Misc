namespace Rhynia.Misc.Patch;

internal class PatchBase_Biotech(Harmony harmony) : PatchBase(harmony)
{
    public override string Name => "Biotech";
    public override string ModId => "Ludeon.Rimworld.Biotech";
    public override string LogLabel => "Rhynia.Misc";

    protected override bool ShouldApply => ModsConfig.BiotechActive;

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "Biotech_NoSpawnCustomXenotype",
                () => SettingsProxy.GetBool("Biotech_NoSpawnCustomXenotype"),
                () =>
                    AccessTools.Method(
                        typeof(PawnGenerator),
                        nameof(PawnGenerator.AdjustXenotypeForFactionlessPawn)
                    ),
                () =>
                    new(
                        typeof(Patch_Biotech_NoSpawnCustomXenotype),
                        nameof(Patch_Biotech_NoSpawnCustomXenotype.Prefix)
                    ),
                HarmonyPatchType.Prefix
            ),
        ];
}
