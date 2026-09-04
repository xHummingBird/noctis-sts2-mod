using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using Noctis.NoctisCode.Cards.Ancient;
using Noctis.NoctisCode.Cards.Basic;
using Noctis.NoctisCode.Cards.Common;
using Noctis.NoctisCode.Relics;

namespace Noctis.NoctisCode.Extensions;

[HarmonyPatch(typeof(TouchOfOrobas), "GetUpgradedStarterRelic")]
internal static class NoctisTouchOfOrobasPatch
{
    private static void Postfix(RelicModel starterRelic, ref RelicModel __result)
    {
        if (starterRelic is RingOfTheLucii)
        {
            __result = ModelDb.Relic<FoundersKingSigil>().ToMutable();
        }
    }
}


[HarmonyPatch(typeof(ArchaicTooth), "TranscendenceUpgrades", MethodType.Getter)]
internal static class NoctisArchaicToothTranscendencePatch
{
    [HarmonyPostfix]
    private static void Postfix(ref Dictionary<ModelId, CardModel> __result)
    {
        __result[ModelDb.Card<SwordOfTheWise>().Id] = ModelDb.Card<SwordOfTheFather>();
    }
}

[HarmonyPatch(typeof(DustyTome), nameof(DustyTome.SetupForPlayer))]
public static class DustyTomeSetupPatch
{
    [HarmonyPostfix]
    public static void Postfix(DustyTome __instance, Player player)
    {
        if (player.Character is not Character.Noctis)
            return;

        __instance.AncientCard = ModelDb.Card<WarpStrikeRush>().Id;
    }
}

[HarmonyPatch(typeof(DustyTome), nameof(DustyTome.AfterObtained))]
public static class DustyTomePatch
{
    [HarmonyPrefix]
    public static void Prefix(DustyTome __instance)
    {
        if (__instance.Owner?.Character is not Character.Noctis)
            return;
        
        __instance.AncientCard = ModelDb.Card<WarpStrikeRush>().Id;
    }
}