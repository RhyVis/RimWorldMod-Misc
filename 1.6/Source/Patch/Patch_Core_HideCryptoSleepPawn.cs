namespace Rhynia.Misc.Patch;

internal static class Patch_Core_HideCryptoSleepPawn
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var method = AccessTools.Method(typeof(ColonistBar), "CheckRecacheEntries");
            if (method is null)
            {
                Error(
                    "Failed to apply patch Core_HideCryptoSleepPawn: Could not find method ColonistBar.HideCryptoSleepPawn"
                );
            }

            harmony.Patch(
                method,
                transpiler: new(typeof(Patch_Core_HideCryptoSleepPawn), nameof(Transpiler))
            );

            Info("Applied patch Core_HideCryptoSleepPawn");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_HideCryptoSleepPawn: {ex.Message}");
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);

        var targetGetter = AccessTools.PropertyGetter(
            typeof(MapPawns),
            nameof(MapPawns.FreeColonists)
        );
        if (targetGetter is null)
        {
            Error(
                "Failed to apply patch Core_HideCryptoSleepPawn: Could not find property MapPawns.FreeColonists"
            );
            return codes;
        }

        var processorMethod = AccessTools.Method(
            typeof(Patch_Core_HideCryptoSleepPawn_Helper),
            nameof(Patch_Core_HideCryptoSleepPawn_Helper.ProcessFreeColonists)
        );
        if (processorMethod is null)
        {
            Error(
                "Failed to apply patch Core_HideCryptoSleepPawn: Could not find method Patch_Core_HideCryptoSleepPawn_Helper.ProcessFreeColonists"
            );
            return codes;
        }

        bool patched = false;
        for (int i = 0; i < codes.Count; i++)
            if (
                codes[i].opcode == OpCodes.Callvirt
                && codes[i].operand is MethodInfo method
                && method == targetGetter
            )
            {
                codes.Insert(i + 1, new(OpCodes.Call, processorMethod));
                patched = true;
                break;
            }

        if (!patched)
            Warn("Could not find target instruction to patch in ColonistBar.CheckRecacheEntries");

        return codes;
    }
}

public static class Patch_Core_HideCryptoSleepPawn_Helper
{
    public static List<Pawn> ProcessFreeColonists(List<Pawn> freeColonists) =>
        [.. freeColonists.Where(pawn => pawn.ParentHolder is not Building_CryptosleepCasket)];
}
