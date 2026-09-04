using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class KingsBurden() : NoctisCard(1, CardType.Power,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(2),
        new PowerVar<StrengthPower>(2m),
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>()
    ];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, 2, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.IntValue,
            base.Owner.Creature, this);
        for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
        {
            await WarpStrike.CreateInHand(base.Owner, base.CombatState);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Strength.UpgradeValueBy(1m);
    }
}