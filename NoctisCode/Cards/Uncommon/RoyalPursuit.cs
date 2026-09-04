using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class RoyalPursuit() : NoctisCard(0, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WarpStrikeNextTurnPower>(1m),
        new EnergyVar(1)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
        await PowerCmd.Apply<WarpStrikeNextTurnPower>(choiceContext, base.Owner.Creature, DynamicVars["WarpStrikeNextTurnPower"].BaseValue, base.Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["WarpStrikeNextTurnPower"].UpgradeValueBy(1);
    }
}