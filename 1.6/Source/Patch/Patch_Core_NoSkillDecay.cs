namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoSkillDecay
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn)) is { } method)
            {
                harmony.Patch(method, prefix: new(typeof(Patch_Core_NoSkillDecay), nameof(Prefix)));
                Info("Applied patch Core_NoSkillDecay");
            }
            else
                Error(
                    "Failed to apply patch Core_NoSkillDecay: Could not find method SkillRecord.Learn"
                );
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Core_NoSkillDecay: {ex}");
        }
    }

    internal static void Prefix(ref float xp)
    {
        if (xp < 0f)
            xp = 0f;
    }
}
