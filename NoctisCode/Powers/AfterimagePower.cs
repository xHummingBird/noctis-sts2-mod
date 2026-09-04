using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Noctis.NoctisCode.Powers;

public class AfterimagePower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == base.Owner && dealer != null && (props.IsPoweredAttack() || cardSource is Omnislice))
        {
            Flash();
            await CreatureCmd.Damage(choiceContext, dealer, base.Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, base.Owner, null, null);
        }
    }
    
    public override async Task AfterSideTurnStartLate(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != base.Owner.Side)
            return;
        
        await PowerCmd.Remove(this);
    }
}
