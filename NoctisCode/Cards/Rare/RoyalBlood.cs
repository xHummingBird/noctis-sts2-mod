using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Cards.Rare;

public class RoyalBlood() : NoctisCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2),
        new EnergyVar(2),
        new HpLossVar(5),
        new DynamicVar("Armiger", 5)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        ArmigerManager.GainArmiger(Owner, DynamicVars["Armiger"].IntValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
        base.DynamicVars["Armiger"].UpgradeValueBy(3m);
    }
}