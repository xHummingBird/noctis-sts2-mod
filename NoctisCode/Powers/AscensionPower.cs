using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Powers;

public class AscensionPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != base.Owner || result.UnblockedDamage <= 0 || base.Owner.CombatState.CurrentSide != base.Owner.Side)
        {
            return;
        }
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            NFireBurstVfx child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        }
        await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
        ArmigerManager.GainArmiger(Owner.Player, base.Amount);
        
        await Owner.CheckArmigerReady(
            choiceContext,
            Owner,
            null
        );
    }
}
    
