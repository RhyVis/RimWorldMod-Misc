using Verse.AI;

namespace Rhynia.Misc.Patch;

internal class PatchBase_Core : PatchBase
{
    public override string Name => "Core";
    public override string ModId => "Ludeon.Rimworld";
    public override string LogLabel => "Rhynia.Misc";

    protected override bool ShouldApply => true;

    protected override IEnumerable<PatchProvider> PatchProviders =>
        [
            new(
                "Core_HideCryptoSleepPawn",
                () => SettingsProxy.GetBool("Core_HideCryptoSleepPawn"),
                () => AccessTools.Method(typeof(ColonistBar), "CheckRecacheEntries"),
                () =>
                    new(
                        typeof(Patch_Core_HideCryptoSleepPawn),
                        nameof(Patch_Core_HideCryptoSleepPawn.Transpiler)
                    ),
                HarmonyPatchType.Transpiler
            ),
            new(
                "Core_NonPlayerDoorHoldOpen",
                () => SettingsProxy.GetBool("Core_NonPlayerDoorHoldOpen"),
                () => AccessTools.Method(typeof(Building_Door), nameof(Building_Door.SpawnSetup)),
                () =>
                    new(
                        typeof(Patch_Core_NonPlayerDoorHoldOpen),
                        nameof(Patch_Core_NonPlayerDoorHoldOpen.Postfix)
                    ),
                HarmonyPatchType.Postfix
            ),
            new(
                "Core_NoPlantRest",
                () => SettingsProxy.GetBool("Core_NoPlantRest"),
                () => AccessTools.PropertyGetter(typeof(Plant), "Resting"),
                () => new(typeof(Patch_Core_NoPlantRest), nameof(Patch_Core_NoPlantRest.Prefix)),
                HarmonyPatchType.Prefix
            ),
            new(
                "Core_NoSkillDecay",
                () => SettingsProxy.GetBool("Core_NoSkillDecay"),
                () => AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn)),
                () => new(typeof(Patch_Core_NoSkillDecay), nameof(Patch_Core_NoSkillDecay.Prefix)),
                HarmonyPatchType.Prefix
            ),
            new(
                "Core_NoSurgeryFail",
                () => SettingsProxy.GetBool("Core_NoSurgeryFail"),
                () => AccessTools.Method(typeof(Recipe_Surgery), "CheckSurgeryFail"),
                () =>
                    new(typeof(Patch_Core_NoSurgeryFail), nameof(Patch_Core_NoSurgeryFail.Prefix)),
                HarmonyPatchType.Prefix
            ),
            new(
                "Core_NoTurretConsume",
                () => SettingsProxy.GetBool("Core_NoTurretConsume"),
                () =>
                    AccessTools.Method(typeof(CompRefuelable), nameof(CompRefuelable.ConsumeFuel)),
                () =>
                    new(
                        typeof(Patch_Core_NoTurretConsume),
                        nameof(Patch_Core_NoTurretConsume.Prefix)
                    ),
                HarmonyPatchType.Prefix
            ),
            new(
                "Core_PawnDeathNoImmediateRot",
                () => SettingsProxy.GetBool("Core_PawnDeathNoImmediateRot"),
                () => AccessTools.Method(typeof(Pawn), nameof(Pawn.Kill)),
                () =>
                    new(
                        typeof(Patch_Core_PawnDeathNoImmediateRot),
                        nameof(Patch_Core_PawnDeathNoImmediateRot.Transpiler)
                    ),
                HarmonyPatchType.Transpiler
            ),
            new(
                "Core_RejectRescueJoin_Patch1",
                () => SettingsProxy.GetBool("Core_RejectRescueJoin"),
                () =>
                    AccessTools.Method(
                        typeof(Pawn_MindState),
                        nameof(Pawn_MindState.JoinColonyBecauseRescuedBy)
                    ),
                () =>
                    new(
                        typeof(Patch_Core_RejectRescueJoin),
                        nameof(
                            Patch_Core_RejectRescueJoin.PawnMindState_JoinColonyBecauseRescuedBy_Prefix
                        )
                    ),
                HarmonyPatchType.Prefix
            ),
            new(
                "Core_RejectRescueJoin_Patch2",
                () => SettingsProxy.GetBool("Core_RejectRescueJoin"),
                () => AccessTools.Method(typeof(Pawn_GuestTracker), "Notify_PawnUndowned"),
                () =>
                    new(
                        typeof(Patch_Core_RejectRescueJoin),
                        nameof(
                            Patch_Core_RejectRescueJoin.PawnGuestTracker_NotifyPawnUndowned_Transpiler
                        )
                    ),
                HarmonyPatchType.Transpiler
            ),
        ];
}
