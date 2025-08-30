namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoTurretConsume
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (
                AccessTools.Method(typeof(CompRefuelable), nameof(CompRefuelable.ConsumeFuel)) is
                { } method
            )
            {
                harmony.Patch(
                    method,
                    prefix: new(typeof(Patch_Core_NoTurretConsume), nameof(Prefix))
                );
                Info("Applied patch Core_NoTurretConsume");
            }
            else
                Error("Failed to find method CompRefuelable.ConsumeFuel");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_NoTurretConsume: {ex}");
        }
    }

    internal static bool Prefix(CompRefuelable __instance) =>
        !(__instance.parent is { Faction.IsPlayer: true, def.building.IsTurret: true });
}
