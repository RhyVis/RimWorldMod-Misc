using Rhynia.Misc.Patch;
using XmlExtensions;

namespace Rhynia.Misc;

[StaticConstructorOnStartup]
public class Mod_Misc(ModContentPack mod) : Mod(mod)
{
    internal const string MOD_ID = "Rhynia.Mod.Misc";

    private static readonly Harmony harmony = new(MOD_ID);

    static Mod_Misc()
    {
        using var _ = TimingScope.Start(
            (elapsed) => Info($"Initialized in {elapsed.Milliseconds} ms")
        );

        harmony.Apply<PatchBase_Core>();
        harmony.Apply<PatchBase_Ideology>();
        harmony.Apply<PatchBase_Biotech>();
        harmony.Apply<PatchBase_Odyssey>();
        harmony.Apply<PatchBase_CTA>();
        harmony.Apply<PatchBase_FA>();
        harmony.Apply<PatchBase_Addon>();
    }
}

internal static class SettingsProxy
{
    internal static bool GetBool(string key, bool defaultValue = false) =>
        bool.TryParse(SettingsManager.GetSetting(Mod_Misc.MOD_ID, key), out var result)
            ? result
            : defaultValue;
}

[LoggerLabel("Rhynia.Misc")]
internal struct LogLabel;
