namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoPlantRest
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (AccessTools.PropertyGetter(typeof(Plant), "Resting") is { } method)
            {
                harmony.Patch(method, prefix: new(typeof(Patch_Core_NoPlantRest), nameof(Prefix)));
                Info("Applied patch Patch_Core_NoPlantRest");
            }
            else
                Error(
                    "Failed to apply patch Core_NoPlantRest: Could not find method Plant.Resting"
                );
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Patch_Core_NoPlantRest: {ex}");
        }
    }

    internal static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}
