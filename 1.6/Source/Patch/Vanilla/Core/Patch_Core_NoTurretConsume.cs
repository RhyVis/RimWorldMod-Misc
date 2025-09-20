namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoTurretConsume
{
    internal static bool Prefix(CompRefuelable __instance) =>
        !(__instance.parent is { Faction.IsPlayer: true, def.building.IsTurret: true });
}
