namespace Rhynia.Misc.Util;

internal static class Patch_Core_NoSkillDecay
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            var method = AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn));
            if (method is null)
            {
                Out.Error(
                    "Failed to apply patch Core_NoSkillDecay: Could not find method SkillRecord.Learn"
                );
                return;
            }

            harmony.Patch(method, prefix: new(typeof(Patch_Core_NoSkillDecay), nameof(Prefix)));

            Out.Info("Applied patch Core_NoSkillDecay");
        }
        catch (Exception ex)
        {
            Out.Error($"Failed to apply patch Core_NoSkillDecay: {ex}");
        }
    }

    internal static void Prefix(ref float xp)
    {
        if (xp < 0f)
            xp = 0f;
    }
}
