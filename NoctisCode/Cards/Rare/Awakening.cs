using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Noctis.NoctisCode.Cards.Token;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Rare;

//Add 2 WarpStrikes + the upgrade and warp strike now deal damage to the rest of the enemies
public class Awakening() : NoctisCard(2, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<AwakeningPower>(2),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<AwakeningPower>(),
        HoverTipFactory.FromCard<WarpStrike>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();

        await PowerCmd.Apply<AwakeningPower>(
            choiceContext,
            base.Owner.Creature,
            1,
            base.Owner.Creature,
            this);
        for (int i = 0; i < base.DynamicVars["AwakeningPower"].IntValue; i++)
        {
            await WarpStrike.CreateInHand(base.Owner, base.CombatState);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["AwakeningPower"].UpgradeValueBy(1);
    }
}