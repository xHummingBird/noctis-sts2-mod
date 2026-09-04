using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class KingsLegacy() : NoctisCard(2, CardType.Power,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<KingsLegacyPower>(1),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<KingsLegacyPower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();

        await PowerCmd.Apply<KingsLegacyPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["KingsLegacyPower"].BaseValue,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}