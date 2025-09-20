namespace Rhynia.Misc.Patch;

internal static class Patch_Ideology_FastRelic
{
    internal static void Prefix(QuestPart_SubquestGenerator __instance, ref int? ___currentInterval)
    {
        if (__instance is not QuestPart_SubquestGenerator_RelicHunt)
            return;
        __instance.interval = new(50_000, 100_000);
        if (___currentInterval != null)
            ___currentInterval = __instance.interval.RandomInRange;
    }
}
