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

    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (
                AccessTools.Method(
                    typeof(LetterStack),
                    nameof(LetterStack.ReceiveLetter),
                    [typeof(Letter), typeof(string), typeof(int), typeof(bool)]
                ) is
                { } method
            )
            {
                harmony.Patch(
                    method,
                    postfix: new(typeof(Patch_Addon_ScreamingIncident), nameof(Postfix))
                );
                Info("Applied patch Addon_ScreamingIncident");
            }
            else
                Error(
                    "Failed to apply patch Addon_ScreamingIncident: Could not find method LetterStack.ReceiveLetter(Letter, string, int, bool)"
                );
        }
        catch (Exception ex)
        {
            Error($"Failed to apply patch Addon_ScreamingIncident: {ex}");
        }
    }

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
