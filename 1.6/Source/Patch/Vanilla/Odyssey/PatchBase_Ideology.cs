using RimWorld.Planet;

namespace Rhynia.Misc.Patch;

internal class PatchBase_Odyssey(Harmony harmony) : PatchBase(harmony)
{
    public override string Name => "Odyssey";
    public override string ModId => "Ludeon.Rimworld.Odyssey";
    public override string LogLabel => "Rhynia.Misc";

    protected override bool ShouldApply => ModsConfig.OdysseyActive;

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "Odyssey_HeadingNorth",
                () => SettingsProxy.GetBool("Odyssey_HeadingNorth"),
                () => AccessTools.Method(typeof(Gravship), "DetermineLaunchDirection"),
                () =>
                    new(
                        typeof(Patch_Odyssey_HeadingNorth),
                        nameof(Patch_Odyssey_HeadingNorth.Prefix)
                    ),
                HarmonyPatchType.Prefix
            ),
        ];
}
