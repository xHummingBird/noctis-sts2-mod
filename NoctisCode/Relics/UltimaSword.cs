using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using Noctis.NoctisCode.Cards.Token;

namespace Noctis.NoctisCode.Relics;

public class UltimaSword : NoctisRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("WarpStrikes", 3m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>()
    ];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player == base.Owner && base.Owner.PlayerCombatState.TurnNumber <= 1)
        {
            Flash();
            for (int i = 0; i < 3; i++)
            {
                await WarpStrike.CreateInHand(base.Owner, combatState);
                await Cmd.Wait(0.25f);
            }
        }
    }
}