using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Mechanic.Armiger;

namespace Noctis.NoctisCode.Powers;

public class ArmigerMasteryPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        foreach (CardModel item in base.Owner.Player.PlayerCombatState.AllCards.Where((CardModel c) => c is IRoyalArmsCard))
        {
            CardCmd.ApplyKeyword(item, CardKeyword.Retain);
        }
        return Task.CompletedTask;
    }
    
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(base.Owner))
        {
            return;
        }
        CardPile pile = PileType.Discard.GetPile(base.Owner.Player);
        IEnumerable<CardModel> source = pile.Cards.Where((CardModel c) => c is IRoyalArmsCard);
        IEnumerable<CardModel> enumerable = source.ToList().UnstableShuffle(base.Owner.Player.RunState.Rng.CombatCardSelection).Take(base.Amount);
        foreach (CardModel card in enumerable)
        {
            await CardPileCmd.Add(card, PileType.Hand);
            if (card.IsUpgradable)
            {
                CardCmd.Upgrade(card);
            }
        }
    }
}