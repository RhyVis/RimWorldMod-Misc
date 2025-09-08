namespace Rhynia.Misc.Patch;

internal class Patch_Core_NonPlayerDoorHoldOpen
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (
                AccessTools.Method(typeof(Building_Door), nameof(Building_Door.SpawnSetup)) is
                { } method
            )
            {
                harmony.Patch(
                    method,
                    postfix: new(typeof(Patch_Core_NonPlayerDoorHoldOpen), nameof(Postfix))
                );
                Info("Applied patch Core_NonPlayerDoorHoldOpen");
            }
            else
            {
                Error("Failed to find method Building_Door.SpawnSetup");
            }
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_NonPlayerDoorHoldOpen: {ex}");
        }
    }

    static void Postfix(Building_Door __instance, ref bool ___holdOpenInt)
    {
        if (__instance.Faction != Faction.OfPlayer)
            ___holdOpenInt = true;
    }
}
