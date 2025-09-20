using Verse.Sound;

namespace Rhynia.Misc.Patch;

internal static class Patch_Addon_ScreamingIncident
{
    private static readonly Dictionary<LetterDef, SoundDef> _map = new()
    {
        { LetterDefOf.ThreatBig, DefOf_Misc.RhyMisc_Sound_WaAo },
        { LetterDefOf.ThreatSmall, DefOf_Misc.RhyMisc_Sound_WaAo },
        { LetterDefOf.NeutralEvent, DefOf_Misc.RhyMisc_Sound_YaHo },
        { LetterDefOf.PositiveEvent, DefOf_Misc.RhyMisc_Sound_YaHo },
    };

    internal static void Postfix(Letter __0)
    {
        if (_map.TryGetValue(__0.def, out var sound))
            try
            {
                sound.PlayOneShotOnCamera();
            }
            catch (Exception ex)
            {
                Error($"Failed to play sound for letter {__0}: {ex}");
            }
    }
}
