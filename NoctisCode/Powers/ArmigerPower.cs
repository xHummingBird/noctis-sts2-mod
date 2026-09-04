using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Powers;

public sealed class ArmigerPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (base.Owner != dealer)
            return 0m;

        if (!props.IsPoweredAttack())
            return 0m;

        if (cardSource is not IWarpStrikeCard)
            return 0m;

        return 4m;
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        var ownerCreature = Owner;
        var noctis = Owner?.Player.Character as Character.Noctis;
        if (ownerCreature != null && noctis != null)
        {
            AudioHelper.PlayRandomLimitBreak();
            SfxCmd.Play("res://Noctis/sfx/weapon_appear_1.wav");
            float duration = noctis.PlayAnimation(ownerCreature, "transform_armiger").total;
            await Task.Delay((int)(0.6f * 1000f));
            noctis.PlayAnimation(Owner.Player.Creature, "idle_armiger");
        }
        await PowerCmd.Apply<ArmigerArtsReadyPower>(new ThrowingPlayerChoiceContext(), Owner, 1, Owner, null);
    }
    
    public override async Task AfterSideTurnStartLate(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != base.Owner.Side)
            return;
        
        if (!Owner.HasPower<ArmigerArtsReadyPower>())
            await PowerCmd.Apply<ArmigerArtsReadyPower>(new ThrowingPlayerChoiceContext(), Owner, 1, Owner, null);
        
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player)
            return;
        
        if (Amount <= 1)
        {
            await PowerCmd.Decrement(this);
            var ownerCreature = Owner;
            var noctis = Owner?.Player.Character as Character.Noctis;
            if (ownerCreature != null && noctis != null)
            {
                SfxCmd.Play("res://Noctis/sfx/weapon_disappear_1.wav");
                noctis.PlayAnimation(ownerCreature, "deform_armiger");
                await Task.Delay((int)(0.6f * 1000f));
                noctis.PlayAnimation(ownerCreature, "idle_normal");
            }
            ArmigerManager.SetArmiger(Owner.Player, 0);
            await PowerCmd.Remove<ArmigerArtsReadyPower>(Owner);
        }
        else
        {
            await PowerCmd.Decrement(this);
        }
    }
}