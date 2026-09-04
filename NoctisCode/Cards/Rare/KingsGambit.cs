using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Rare;

//Discard all your hand. next turn, add warp strike the same amount discarded
public class KingsGambit() : NoctisCard(1, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();
        IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(base.Owner).Cards.ToList();
        int handSize = enumerable.Count();
        await CardCmd.Discard(choiceContext, enumerable);
        await PowerCmd.Apply<WarpStrikeNextTurnPower>(
            choiceContext,
            Owner.Creature,
            handSize,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
    
    
    
    
