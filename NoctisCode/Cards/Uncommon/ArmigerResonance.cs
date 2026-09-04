using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Mechanic.Armiger;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class ArmigerResonance() : NoctisCard(0, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        IEnumerable<CardModel> cards = (await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner)).Where(c => c is not IRoyalArmsCard);
        await CardCmd.Discard(choiceContext, cards);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}