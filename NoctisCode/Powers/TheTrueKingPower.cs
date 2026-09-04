using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Mechanic.WarpStrike;

namespace Noctis.NoctisCode.Powers;

//The first Warp Strike you play deal 5(7) more damage
public class TheTrueKingPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>(),
    ];

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }
        if (cardSource == null || !(cardSource is IWarpStrikeCard))
        {
            return 0m;
        }
        if (dealer != base.Owner)
        {
            return 0m;
        }
        int num = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card is IWarpStrikeCard && e.CardPlay.Player == base.Owner.Player);
        if (num > 0)
        {
            return 0m;
        }
        return base.Amount;
    }
}