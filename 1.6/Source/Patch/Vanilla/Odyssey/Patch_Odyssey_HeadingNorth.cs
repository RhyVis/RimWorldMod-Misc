using RimWorld.Planet;

namespace Rhynia.Misc.Patch;

internal static class Patch_Odyssey_HeadingNorth
{
    internal static bool Prefix(Gravship __instance)
    {
        if (__instance.PilotConsole is null && __instance.launchDirection == IntVec3.Zero)
        {
            __instance.launchDirection = IntVec3.North;
            return false;
        }
        else
            return true;
    }
}
