namespace Rhynia.Misc.Patch;

internal static class Patch_Biotech_NoSpawnCustomXenotype
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            harmony.Patch(
                original: AccessTools.Method(
                    typeof(PawnGenerator),
                    nameof(PawnGenerator.AdjustXenotypeForFactionlessPawn)
                ),
                prefix: new(typeof(Patch_Biotech_NoSpawnCustomXenotype), nameof(Prefix))
            );

            Out.Info("Applied patch Biotech_NoSpawnCustomXenotype.");
        }
        catch (Exception ex)
        {
            Out.Error($"Failed to apply patch Biotech_NoSpawnCustomXenotype: {ex}");
        }
    }

    static bool Prefix(Pawn pawn, ref XenotypeDef xenotype)
    {
        Out.Debug($"AdjustXenotypeForFactionlessPawn: {pawn.Name} ({pawn.GetType()})");

        if (
            DefDatabase<XenotypeDef>.AllDefs.TryRandomElementByWeight(
                x => x.factionlessGenerationWeight,
                out var picked
            )
        )
            xenotype = picked;

        return false;
    }
}
