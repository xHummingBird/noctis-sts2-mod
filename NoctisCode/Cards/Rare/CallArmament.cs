using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Noctis.NoctisCode.Character;
using Noctis.NoctisCode.Mechanic.Armiger;

namespace Noctis.NoctisCode.Cards.Rare;

public class CallArmament() : NoctisCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? cardModel = CardFactory.GetDistinctForCombat(base.Owner, from c in ModelDb.CardPool<NoctisCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
            where c is IRoyalArmsCard
            select c, 1, base.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (cardModel != null)
        {
            cardModel.SetToFreeThisTurn();
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}