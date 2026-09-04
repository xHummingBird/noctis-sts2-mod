using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class RingsToll() : NoctisCard(0, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>()
    ];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, 2, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
        {
            await WarpStrike.CreateInHand(base.Owner, base.CombatState);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}