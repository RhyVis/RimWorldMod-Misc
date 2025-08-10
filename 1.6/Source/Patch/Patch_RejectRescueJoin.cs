using System.Reflection.Emit;
using Verse.AI;

namespace Rhynia.Misc.Patch;

internal static class Patch_RRJ
{
    public static void Apply(Harmony harmony)
    {
        try
        {
            harmony.Patch(
                original: AccessTools.Method(
                    typeof(Pawn_MindState),
                    nameof(Pawn_MindState.JoinColonyBecauseRescuedBy)
                ),
                prefix: new(
                    typeof(Patch_RRJ_PawnMindState_JoinColonyBecauseRescuedBy),
                    nameof(Patch_RRJ_PawnMindState_JoinColonyBecauseRescuedBy.Prefix)
                )
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(Pawn_GuestTracker), "Notify_PawnUndowned"),
                transpiler: new(
                    typeof(Patch_RRJ_PawnGuestTracker_NotifyPawnUndowned),
                    nameof(Patch_RRJ_PawnGuestTracker_NotifyPawnUndowned.Transpiler)
                )
            );
            Out.Info("Applied Patch_RRJ successfully.");
        }
        catch (Exception ex)
        {
            Out.Error($"Failed to apply Patch_RRJ: {ex}");
        }
    }
}

internal static class Patch_RRJ_PawnMindState_JoinColonyBecauseRescuedBy
{
    internal static bool Prefix(Pawn_MindState __instance)
    {
        __instance.WillJoinColonyIfRescued = false;
        return false;
    }
}

internal static class Patch_RRJ_PawnGuestTracker_NotifyPawnUndowned
{
    internal static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
    {
        bool foundValueSeeded = false;

        foreach (var instruction in instructions)
        {
            if (
                !foundValueSeeded
                && instruction.opcode == OpCodes.Call
                && instruction.operand?.ToString().Contains("ValueSeeded") == true
            )
            {
                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Ldc_R4, 1.0f);
                foundValueSeeded = true;
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
