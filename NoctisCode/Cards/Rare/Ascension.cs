using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Rare;

public class Ascension() : NoctisCard(1, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<AscensionPower>(5)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<AscensionPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();

        await PowerCmd.Apply<AscensionPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["AscensionPower"].IntValue,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["AscensionPower"].UpgradeValueBy(2);
    }
}