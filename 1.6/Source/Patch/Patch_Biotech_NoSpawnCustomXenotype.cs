namespace Rhynia.Misc.Patch;

internal static class Patch_Biotech_NoSpawnCustomXenotype
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var method = AccessTools.Method(
                typeof(PawnGenerator),
                nameof(PawnGenerator.AdjustXenotypeForFactionlessPawn)
            );
            if (method is null)
            {
                Error("Method PawnGenerator.AdjustXenotypeForFactionlessPawn not found.");
                return;
            }

            harmony.Patch(
                original: method,
                prefix: new(typeof(Patch_Biotech_NoSpawnCustomXenotype), nameof(Prefix))
            );

            Info("Applied patch Biotech_NoSpawnCustomXenotype.");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Biotech_NoSpawnCustomXenotype: {ex}");
        }
    }

    static bool Prefix(Pawn pawn, ref XenotypeDef xenotype)
    {
        Debug($"AdjustXenotypeForFactionlessPawn: {pawn.Name} ({pawn.GetType()})");

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
