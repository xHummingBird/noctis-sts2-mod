using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Noctis.NoctisCode.Mechanic.Armiger;

namespace Noctis.NoctisCode.Powers;

public class CrossChainPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != base.Owner)
            return playCount;

        if (card is not IRoyalArmsCard)
            return playCount;

        int numPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count(
            e =>
                e.Actor == base.Owner &&
                e.CardPlay.IsFirstInSeries &&
                e.HappenedThisTurn(base.CombatState) &&
                e.CardPlay.Card is IRoyalArmsCard
        );

        if (numPlayedThisTurn >= base.Amount)
            return playCount;

        return playCount + 1;
    }
    
    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        return Task.CompletedTask;
    }
}