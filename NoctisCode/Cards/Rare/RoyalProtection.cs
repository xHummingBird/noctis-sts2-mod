using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Cards.Rare;

//Block Card. Gain 8 block, 1 cost. gain 2 block for all your royal arms card.
public class RoyalProtection() : NoctisCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(8m),
        new CalculationExtraVar(1m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => Math.Max(0, card.Owner.PlayerCombatState.AllCards.Count((CardModel c) => c is IRoyalArmsCard)))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.CalculationBase.UpgradeValueBy(2m);
        base.DynamicVars.CalculationExtra.UpgradeValueBy(1m);
    }
}