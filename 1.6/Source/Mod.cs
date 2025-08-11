using Rhynia.Misc.Patch;
using XmlExtensions;

namespace Rhynia.Misc;

[StaticConstructorOnStartup]
public class Mod_Misc(ModContentPack mod) : Mod(mod)
{
    internal const string MOD_ID = "Rhynia.Mod.Misc";

    static readonly Harmony harmony = new(MOD_ID);

    static Mod_Misc()
    {
        Optional("Core_RejectRescueJoin", Patch_Core_RejectRescueJoin.Apply);
        Optional("Core_NoSkillDecay", Patch_Core_NoSkillDecay.Apply);
        Optional("Core_NoSurgeryFail", Patch_Core_NoSurgeryFail.Apply);
        Optional("Core_NoTurretConsume", Patch_Core_NoTurretConsume.Apply);

        Optional(
            "Biotech_NoSpawnCustomXenotype",
            Patch_Biotech_NoSpawnCustomXenotype.Apply,
            "Ludeon.RimWorld.Biotech"
        );

        Optional("FA_PawnUpdateExtend", Patch_FA_PawnUpdateExtend.Apply, "Nals.FacialAnimation");

        Optional(
            "CTA_SmallerFacility",
            Patch_CTA_SmallerFacility.Apply,
            "tot.celetech.mkiii",
            "Rhynia.Misc.Patch_CTA_SmallerFacility"
        );

        Out.Info("Mod Rhynia Misc initialized.");
    }

    static void Optional(
        string key,
        Action<Harmony> action,
        string? modPackageId = null,
        string? queueEvent = null,
        bool asynchronously = false
    )
    {
        if (modPackageId is not null && !ModsConfig.IsActive(modPackageId))
            return;
        if (Settings.GetBool(key))
            if (queueEvent is not null)
                LongEventHandler.QueueLongEvent(
                    () => action.Invoke(harmony),
                    queueEvent,
                    asynchronously,
                    (ex) => Out.Error($"Failed to queue {queueEvent} event: {ex}")
                );
            else
                action.Invoke(harmony);
    }
}

internal static class Settings
{
    internal static bool GetBool(string key, bool defaultValue = false) =>
        bool.TryParse(SettingsManager.GetSetting(Mod_Misc.MOD_ID, key), out var result)
            ? result
            : defaultValue;
}
