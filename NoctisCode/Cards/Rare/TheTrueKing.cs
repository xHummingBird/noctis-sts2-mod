using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Rare;

public class TheTrueKing() : NoctisCard(1, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<TheTrueKingPower>(7),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<TheTrueKingPower>(),
        HoverTipFactory.FromCard<WarpStrike>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();

        await PowerCmd.Apply<TheTrueKingPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["TheTrueKingPower"].BaseValue,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TheTrueKingPower"].UpgradeValueBy(3);
    }
}