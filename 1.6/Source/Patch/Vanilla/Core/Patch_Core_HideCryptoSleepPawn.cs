namespace Rhynia.Misc.Patch;

internal static class Patch_Core_HideCryptoSleepPawn
{
    static List<Pawn> Filter(List<Pawn> freeColonists) =>
        [.. freeColonists.Where(pawn => pawn.ParentHolder is not Building_CryptosleepCasket)];

    internal static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
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
                    AccessTools.Method(typeof(Patch_Core_HideCryptoSleepPawn), nameof(Filter))
                )
            );

        return matcher.InstructionEnumeration();
    }
}
