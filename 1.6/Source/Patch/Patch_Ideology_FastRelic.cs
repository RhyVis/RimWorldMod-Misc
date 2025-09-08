namespace Rhynia.Misc.Patch;

internal static class Patch_Ideology_FastRelic
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (
                AccessTools.Method(
                    typeof(QuestPart_SubquestGenerator),
                    nameof(QuestPart_SubquestGenerator.QuestPartTick)
                ) is
                { } method
            )
            {
                harmony.Patch(
                    method,
                    prefix: new(typeof(Patch_Ideology_FastRelic), nameof(Prefix))
                );
                Info("Applied patch Ideology_FastRelic");
            }
            else
                Error("Failed to find method QuestPart_SubquestGenerator.QuestPartTick");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Ideology_FastRelic: {ex}");
        }
    }

    static void Prefix(QuestPart_SubquestGenerator __instance, ref int? ___currentInterval)
    {
        if (__instance is not QuestPart_SubquestGenerator_RelicHunt)
            return;
        __instance.interval = new(50_000, 100_000);
        if (___currentInterval != null)
            ___currentInterval = __instance.interval.RandomInRange;
    }
}
