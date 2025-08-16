using Verse.AI;

namespace Rhynia.Misc.Patch;

internal static class Patch_Core_RejectRescueJoin
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var method1 = AccessTools.Method(
                typeof(Pawn_MindState),
                nameof(Pawn_MindState.JoinColonyBecauseRescuedBy)
            );
            var method2 = AccessTools.Method(typeof(Pawn_GuestTracker), "Notify_PawnUndowned");

            if (method1 is null || method2 is null)
            {
                Error(
                    $"Methods for patching Core_RejectRescueJoin not found: {method1}, {method2}"
                );
                return;
            }

            harmony.Patch(
                original: method1,
                prefix: new(
                    typeof(Patch_Core_RejectRescueJoin),
                    nameof(PawnMindState_JoinColonyBecauseRescuedBy_Prefix)
                )
            );
            harmony.Patch(
                original: method2,
                transpiler: new(
                    typeof(Patch_Core_RejectRescueJoin),
                    nameof(PawnGuestTracker_NotifyPawnUndowned_Transpiler)
                )
            );

            Info("Applied patch Core_RejectRescueJoin.");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_RejectRescueJoin: {ex}");
        }
    }

    internal static bool PawnMindState_JoinColonyBecauseRescuedBy_Prefix(Pawn_MindState __instance)
    {
        __instance.WillJoinColonyIfRescued = false;
        return false;
    }

    internal static IEnumerable<CodeInstruction> PawnGuestTracker_NotifyPawnUndowned_Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
    {
        bool patched = false;

        foreach (var instruction in instructions)
        {
            if (
                !patched
                && instruction.opcode == OpCodes.Call
                && instruction.operand?.ToString().Contains("ValueSeeded") is true
            )
            {
                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Ldc_R4, 1.0f);
                patched = true;
            }
            else
                yield return instruction;
        }
    }
}
