namespace Rhynia.Misc.Patch;

internal static class Patch_Core_NoSkillDecay
{
    internal static void Prefix(ref float xp)
    {
        if (xp < 0f)
            xp = 0f;
    }
}
