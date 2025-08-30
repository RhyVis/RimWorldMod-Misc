using RimWorld.Planet;

namespace Rhynia.Misc.Patch;

internal static class Patch_Odyssey_HeadingNorth
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var method = AccessTools.Method(typeof(Gravship), "DetermineLaunchDirection");
            if (method is null)
            {
                Error("Failed to find method Gravship.DetermineLaunchDirection");
                return;
            }

            harmony.Patch(method, prefix: new(typeof(Patch_Odyssey_HeadingNorth), nameof(Prefix)));

            Info("Applied patch Odyssey_HeadingNorth");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Odyssey_HeadingNorth: {ex}");
        }
    }

    static bool Prefix(Gravship __instance)
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
