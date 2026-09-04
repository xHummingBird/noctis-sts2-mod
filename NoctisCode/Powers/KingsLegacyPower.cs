using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Powers;

//The first time you use a Royal Arms card, draw 1 card
public class KingsLegacyPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    private bool _canDraw = true;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;

        if (card.Owner.Creature != Owner)
            return;

        if (card is not IRoyalArmsCard)
            return;

        if (!_canDraw)
            return;
        
        Flash();
        var ownerPlayer = base.Owner.Player;
        if (ownerPlayer != null) await CardPileCmd.Draw(choiceContext, Amount, ownerPlayer);
        _canDraw = false;
    }
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        _canDraw = true;
    }
}