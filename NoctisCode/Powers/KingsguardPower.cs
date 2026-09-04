using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Mechanic.Armiger;

namespace Noctis.NoctisCode.Powers;

public class KingsguardPower : NoctisPower
{
    private class Data
    {
        /// <summary>
        /// Keep track of the cards we've seen played and the power amount at the time they were played.
        /// This lets After Image avoid triggering on cards that started play before it was applied, and avoid gaining
        /// extra block on multiple plays of After Image.
        /// </summary>
        public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
            return Task.CompletedTask;

        if (cardPlay.Card is not IRoyalArmsCard)
            return Task.CompletedTask;

        GetInternalData<Data>()
            .amountsForPlayedCards
            .Add(cardPlay.Card, Amount);

        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
            return;

        if (cardPlay.Card is not IRoyalArmsCard)
            return;

        if (GetInternalData<Data>()
                .amountsForPlayedCards
                .Remove(cardPlay.Card, out var value)
            && value > 0)
        {
            await CreatureCmd.GainBlock(
                Owner,
                value,
                ValueProp.Unpowered,
                null,
                fast: true);
        }
    }
}