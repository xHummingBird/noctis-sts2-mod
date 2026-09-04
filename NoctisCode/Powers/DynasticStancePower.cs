using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Powers;

//Gain strength whenever you use a Royal Arms
public class DynasticStancePower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;

        if (card.Owner.Creature != Owner)
            return;

        if (card is not IRoyalArmsCard)
            return;
        
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, null, null);
    }
}