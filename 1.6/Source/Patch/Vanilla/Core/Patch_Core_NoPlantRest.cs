namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoPlantRest
{
    internal static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}
