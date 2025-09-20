using Verse.AI;

namespace Rhynia.Misc.Patch;

internal static class Patch_Core_RejectRescueJoin
{
    internal static bool PawnMindState_JoinColonyBecauseRescuedBy_Prefix(Pawn_MindState __instance)
    {
        __instance.WillJoinColonyIfRescued = false;
        return false;
    }

    internal static IEnumerable<CodeInstruction> PawnGuestTracker_NotifyPawnUndowned_Transpiler(
        IEnumerable<CodeInstruction> instructions
    ) =>
        instructions
            .AsCodeMatcher()
            .MatchEndForward(
                CodeMatch.Calls(AccessTools.Method(typeof(Rand), nameof(Rand.ValueSeeded)))
            )
            .ThrowIfInvalid("Failed to match instructions for patch Core_RejectRescueJoin.")
            .InsertAfter(new(OpCodes.Pop), new(OpCodes.Ldc_R4, 1.0f))
            .InstructionEnumeration();
}
