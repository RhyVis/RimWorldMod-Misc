namespace Rhynia.Misc;

#nullable disable

[DefOf]
public static class DefOf_Misc
{
    public static SoundDef RhyMisc_Sound_WaAo;
    public static SoundDef RhyMisc_Sound_YaHo;

    static DefOf_Misc() => DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_Misc));
}
