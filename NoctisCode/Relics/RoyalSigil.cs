using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Relics;

public class RoyalSigil : NoctisRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (base.Owner.PlayerCombatState.TurnNumber <= 1)
        {
            Flash();
            ArmigerManager.GainArmiger(Owner, 15);
        }
    }
}