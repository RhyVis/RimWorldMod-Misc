namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoSurgeryFail
{
    internal static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}
