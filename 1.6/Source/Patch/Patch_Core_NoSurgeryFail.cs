namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoSurgeryFail
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (AccessTools.Method(typeof(Recipe_Surgery), "CheckSurgeryFail") is { } method)
            {
                harmony.Patch(
                    method,
                    prefix: new(typeof(Patch_Core_NoSurgeryFail), nameof(Prefix))
                );
                Info("Applied patch Core_NoSurgeryFail");
            }
            else
                Error("Failed to find method Recipe_Surgery.CheckSurgeryFail");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_NoSurgeryFail: {ex}");
        }
    }

    internal static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}
