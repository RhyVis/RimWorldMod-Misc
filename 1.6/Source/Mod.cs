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

        OptionalPatch(
            settingKey: "Core_RejectRescueJoin",
            harmonyPatchingAction: Patch_Core_RejectRescueJoin.Apply
        );
        OptionalPatch(
            settingKey: "Core_NoSkillDecay",
            harmonyPatchingAction: Patch_Core_NoSkillDecay.Apply
        );
        OptionalPatch(
            settingKey: "Core_NoSurgeryFail",
            harmonyPatchingAction: Patch_Core_NoSurgeryFail.Apply
        );
        OptionalPatch(
            settingKey: "Core_NoTurretConsume",
            harmonyPatchingAction: Patch_Core_NoTurretConsume.Apply
        );
        OptionalPatch(
            settingKey: "Core_HideCryptoSleepPawn",
            harmonyPatchingAction: Patch_Core_HideCryptoSleepPawn.Apply
        );
        OptionalPatch(
            settingKey: "Core_NoPlantRest",
            harmonyPatchingAction: Patch_Core_NoPlantRest.Apply
        );
        OptionalPatch(
            settingKey: "Core_NonPlayerDoorHoldOpen",
            harmonyPatchingAction: Patch_Core_NonPlayerDoorHoldOpen.Apply
        );
        OptionalPatch(
            settingKey: "Core_PawnDeathNoImmediateRot",
            harmonyPatchingAction: Patch_Core_PawnDeathNoImmediateRot.Apply
        );

        OptionalPatch(
            settingKey: "Patch_Ideology_FastRelic",
            harmonyPatchingAction: Patch_Ideology_FastRelic.Apply
        );

        OptionalPatch(
            settingKey: "Biotech_NoSpawnCustomXenotype",
            harmonyPatchingAction: Patch_Biotech_NoSpawnCustomXenotype.Apply,
            packageId: "Ludeon.RimWorld.Biotech"
        );

        OptionalPatch(
            settingKey: "Odyssey_HeadingNorth",
            harmonyPatchingAction: Patch_Odyssey_HeadingNorth.Apply,
            packageId: "Ludeon.RimWorld.Odyssey"
        );

        OptionalPatch(
            settingKey: "FA_PawnUpdateExtend",
            harmonyPatchingAction: Patch_FA_PawnUpdateExtend.Apply,
            packageId: "Nals.FacialAnimation"
        );

        OptionalPatch(
            settingKey: "CTA_SmallerFacility",
            harmonyPatchingAction: Patch_CTA_SmallerFacility.Apply,
            packageId: "tot.celetech.mkiii",
            queueEventId: "Rhynia.Misc.Patch_CTA_SmallerFacility"
        );

        OptionalPatch(
            settingKey: "Addon_ScreamingIncident",
            harmonyPatchingAction: Patch_Addon_ScreamingIncident.Apply,
            queueEventId: "Patch_Addon_ScreamingIncident"
        );

        DebugPatch(harmony);
    }

    private static void OptionalPatch(
        Action<Harmony> harmonyPatchingAction,
        string? settingKey = null,
        string? packageId = null,
        string? queueEventId = null,
        bool asynchronously = false
    )
    {
        if (packageId is not null && !ModsConfig.IsActive(packageId))
            return;
        else if (settingKey is not null)
            if (!SettingsProxy.GetBool(settingKey))
                return;
            else if (queueEventId is not null)
                LongEventHandler.QueueLongEvent(
                    () => harmonyPatchingAction.Invoke(harmony),
                    queueEventId,
                    asynchronously,
                    (ex) => Error($"Failed to queue {queueEventId} event: {ex}")
                );
            else
                harmonyPatchingAction.Invoke(harmony);
        else
            harmonyPatchingAction.Invoke(harmony);
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
