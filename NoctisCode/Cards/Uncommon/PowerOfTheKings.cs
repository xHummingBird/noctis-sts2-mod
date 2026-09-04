using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class PowerOfTheKings() : NoctisCard(1, CardType.Power,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PowerOfTheKingsPower>(1),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PowerOfTheKingsPower>(),
        HoverTipFactory.FromCard<WarpStrike>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();

        await PowerCmd.Apply<PowerOfTheKingsPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["PowerOfTheKingsPower"].BaseValue,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}