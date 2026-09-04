using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class Kingsguard() : NoctisCard(1, CardType.Power,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<KingsguardPower>(2),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();

        await PowerCmd.Apply<KingsguardPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["KingsguardPower"].BaseValue,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["KingsguardPower"].UpgradeValueBy(1);
    }
}