using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using Noctis.NoctisCode.Cards.Token;

namespace Noctis.NoctisCode.Powers;

public class PowerOfTheKingsPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<WarpStrike>()
    ];
        

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player == base.Owner.Player)
        {
            Flash();
            for (int i = 0; i < Amount; i++)
            {
                await WarpStrike.CreateInHand(base.Owner.Player, combatState);
                await Cmd.Wait(0.25f);
            }
        }
    }
}