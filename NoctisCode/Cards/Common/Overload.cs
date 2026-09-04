using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Common;

//Add 3(4) Warp Strikes. you cannot draw cards this turn.
public class Overload(): NoctisCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>()
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
        {
            await WarpStrike.CreateInHand(base.Owner, base.CombatState);
            await Cmd.Wait(0.25f);
        }
        await PowerCmd.Apply<NoDrawPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}