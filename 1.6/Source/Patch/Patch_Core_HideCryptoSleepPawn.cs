namespace Rhynia.Misc.Patch;

internal static class Patch_Core_HideCryptoSleepPawn
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (AccessTools.Method(typeof(ColonistBar), "CheckRecacheEntries") is { } method)
            {
                harmony.Patch(
                    method,
                    transpiler: new(typeof(Patch_Core_HideCryptoSleepPawn), nameof(Transpiler))
                );
                Info("Applied patch Core_HideCryptoSleepPawn");
            }
            else
                Error(
                    "Failed to apply patch Core_HideCryptoSleepPawn: Could not find method ColonistBar.HideCryptoSleepPawn"
                );
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_HideCryptoSleepPawn: {ex.Message}");
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);

        matcher
            .MatchEndForward(
                new CodeMatch(
                    OpCodes.Callvirt,
                    AccessTools.PropertyGetter(typeof(MapPawns), nameof(MapPawns.FreeColonists))
                )
            )
            .ThrowIfInvalid(
                "Could not find target instruction to patch in ColonistBar.CheckRecacheEntries"
            )
            .InsertAfter(
                new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.Method(
                        typeof(Patch_Core_HideCryptoSleepPawn_Helper),
                        nameof(Patch_Core_HideCryptoSleepPawn_Helper.ProcessFreeColonists)
                    )
                )
            );

        return matcher.InstructionEnumeration();
    }
}

public static class Patch_Core_HideCryptoSleepPawn_Helper
{
    public static List<Pawn> ProcessFreeColonists(List<Pawn> freeColonists) =>
        [.. freeColonists.Where(pawn => pawn.ParentHolder is not Building_CryptosleepCasket)];
}
