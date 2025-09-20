using XmlExtensions;

namespace Rhynia.Misc;

[StaticConstructorOnStartup]
public class Mod_Misc(ModContentPack mod) : Mod(mod)
{
    internal const string MOD_ID = "Rhynia.Mod.Misc";

    static readonly Harmony harmony = new(MOD_ID);

    static Mod_Misc()
    {
        using var _ = TimingScope.Start(
            (elapsed) => Debug($"Mod_Misc initialized in {elapsed.Milliseconds} ms")
        );

        PatchBase.ApplyAll<Mod_Misc>(harmony);
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
