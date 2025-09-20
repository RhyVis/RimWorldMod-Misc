namespace Rhynia.Misc.Patch;

internal static class Patch_Biotech_NoSpawnCustomXenotype
{
    internal static bool Prefix(Pawn pawn, ref XenotypeDef xenotype)
    {
        Debug($"Patch_Biotech_NoSpawnCustomXenotype: {pawn.Name} ({pawn.GetType()})");

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
