using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Noctis.NoctisCode.Relics;

public class CupNoodles : NoctisRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    
    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner && base.Owner.PlayerCombatState.TurnNumber <= 1)
        {
            await CreatureCmd.Heal(base.Owner.Creature, 3);
        }
    }
}