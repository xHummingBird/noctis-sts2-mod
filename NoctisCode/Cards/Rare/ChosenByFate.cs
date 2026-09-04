using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Cards.Rare;

//Gain 25(35) Armiger. Exhaust
public class ChosenByFate() : NoctisCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Armiger", 35),
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArmigerManager.GainArmiger(Owner, DynamicVars["Armiger"].IntValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Armiger"].UpgradeValueBy(15m);
    }
}