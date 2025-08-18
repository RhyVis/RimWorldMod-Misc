namespace Rhynia.Misc.Patch;

internal static class DebugPatch_PipeSystem
{
    const string Namespace = "PipeSystem";
    const string TypeCompResourceStorage = $"{Namespace}.CompResourceStorage";

    internal static void Apply(Harmony harmony)
    {
        if (!ModsConfig.IsActive("OskarPotocki.VanillaFactionsExpanded.Core"))
            return;

        try
        {
            // var typeCompResourceStorage = AccessTools.TypeByName(TypeCompResourceStorage);
            // var propertyGetAmountCanAccept = AccessTools.PropertyGetter(
            //     typeCompResourceStorage,
            //     "AmountCanAccept"
            // );

            // harmony.Patch(
            //     propertyGetAmountCanAccept,
            //     prefix: new(
            //         typeof(DebugPatch_PipeSystem),
            //         nameof(CompResourceStorage_Get_AmountCanAccept_Prefix)
            //     )
            // );

            Debug("Applied debug patch DebugPatch_PipeSystem");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply debug patch DebugPatch_PipeSystem: {ex}");
        }
    }

    // internal static void CompResourceStorage_Get_AmountCanAccept_Prefix(object __instance)
    // {
    //     var isBreakdownable = __instance.ReflectGetField<bool>("isBreakdownable").ToStringOrNull();
    //     var powerComp = __instance.ReflectGetField<object>("powerComp").ToStringOrNull();

    //     Debug($"isBreakdownable: {isBreakdownable}, powerComp: {powerComp}", __instance);
    // }
}
