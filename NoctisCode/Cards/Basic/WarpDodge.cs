using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Basic;

public class WarpDodge() : NoctisCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(5m, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>()
    ];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
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