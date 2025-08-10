using Rhynia.Misc.Patch;
using XmlExtensions;

namespace Rhynia.Misc;

[StaticConstructorOnStartup]
public class Mod_Misc(ModContentPack mod) : Mod(mod)
{
    const string MOD_ID = "Rhynia.Mod.Misc";

    static readonly Harmony harmony = new(MOD_ID);

    static Mod_Misc()
    {
        if (Vanilla_RejectRescueJoin)
            LongEventHandler.QueueLongEvent(
                () => Patch_RRJ.Apply(harmony),
                "Rhynia.Misc.Patch_RRJ",
                true,
                (ex) => Out.Error($"Failed to queue Patch_RRJ event: {ex}")
            );
        if (CMC_SmallerRadar)
            LongEventHandler.QueueLongEvent(
                () => Patch_CMC_SmallerFacility.Apply(harmony),
                "Rhynia.Misc.Patch_CMC_SmallerRadar",
                false,
                (ex) => Out.Error($"Failed to queue Patch_CMC_SmallerRadar event: {ex}")
            );
        Out.Info("Mod Rhynia Misc initialized.");
    }

    internal static bool Vanilla_RejectRescueJoin =>
        bool.TryParse(
            SettingsManager.GetSetting(MOD_ID, "Vanilla_RejectRescueJoin"),
            out var result
        ) && result;

    internal static bool CMC_SmallerRadar =>
        bool.TryParse(SettingsManager.GetSetting(MOD_ID, "CMC_SmallerFacility"), out var result)
        && result;
}
