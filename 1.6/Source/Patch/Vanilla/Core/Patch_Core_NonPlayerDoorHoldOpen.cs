namespace Rhynia.Misc.Patch;

internal class Patch_Core_NonPlayerDoorHoldOpen
{
    internal static void Postfix(Building_Door __instance, ref bool ___holdOpenInt)
    {
        if (__instance.Faction != Faction.OfPlayer)
            ___holdOpenInt = true;
    }
}
