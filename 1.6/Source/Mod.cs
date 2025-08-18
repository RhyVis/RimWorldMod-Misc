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
        using var _ = TimingScope.Start(
            (elapsed) => Debug($"Mod_Misc initialized in {elapsed.Milliseconds} ms")
        );

        OptionalPatch(key: "Core_RejectRescueJoin", action: Patch_Core_RejectRescueJoin.Apply);
        OptionalPatch(key: "Core_NoSkillDecay", action: Patch_Core_NoSkillDecay.Apply);
        OptionalPatch(key: "Core_NoSurgeryFail", action: Patch_Core_NoSurgeryFail.Apply);
        OptionalPatch(key: "Core_NoTurretConsume", action: Patch_Core_NoTurretConsume.Apply);
        OptionalPatch(
            key: "Core_HideCryptoSleepPawn",
            action: Patch_Core_HideCryptoSleepPawn.Apply
        );

        OptionalPatch(
            key: "Biotech_NoSpawnCustomXenotype",
            action: Patch_Biotech_NoSpawnCustomXenotype.Apply,
            packageId: "Ludeon.RimWorld.Biotech"
        );

        OptionalPatch(key: "Odyssey_HeadingNorth", action: Patch_Odyssey_HeadingNorth.Apply);

        OptionalPatch(
            key: "FA_PawnUpdateExtend",
            action: Patch_FA_PawnUpdateExtend.Apply,
            packageId: "Nals.FacialAnimation"
        );

        OptionalPatch(
            key: "CTA_SmallerFacility",
            action: Patch_CTA_SmallerFacility.Apply,
            packageId: "tot.celetech.mkiii",
            queueEventId: "Rhynia.Misc.Patch_CTA_SmallerFacility"
        );

        OptionalPatch(
            key: "Addon_ScreamingIncident",
            action: Patch_Addon_ScreamingIncident.Apply,
            queueEventId: "Patch_Addon_ScreamingIncident"
        );

        DebugPatch(harmony);
    }

    private static void OptionalPatch(
        Action<Harmony> action,
        string? key = null,
        string? packageId = null,
        string? queueEventId = null,
        bool asynchronously = false
    )
    {
        if (packageId is not null && !ModsConfig.IsActive(packageId))
            return;
        else if (key is not null)
            if (!SettingsProxy.GetBool(key))
                return;
            else if (queueEventId is not null)
                LongEventHandler.QueueLongEvent(
                    () => action.Invoke(harmony),
                    queueEventId,
                    asynchronously,
                    (ex) => Error($"Failed to queue {queueEventId} event: {ex}")
                );
            else
                action.Invoke(harmony);
        else
            action.Invoke(harmony);
    }

    private static void DebugPatch(Harmony harmony)
    {
        const bool DEV = false;

        if (!DEV || !Prefs.DevMode)
            return;
        Debug("Starting applying debug patches");

        LongEventHandler.QueueLongEvent(
            () => DebugPatch_PipeSystem.Apply(harmony),
            "Rhynia.Misc.Patch_DebugPatch_PipeSystem",
            false,
            (ex) => Error($"Failed to apply debug patch: {ex}")
        );
    }
}

internal static class SettingsProxy
{
    internal static bool GetBool(string key, bool defaultValue = false) =>
        bool.TryParse(SettingsManager.GetSetting(Mod_Misc.MOD_ID, key), out var result)
            ? result
            : defaultValue;
}

[LoggerLabel("Rhynia.Misc")]
internal struct LogLabel;
