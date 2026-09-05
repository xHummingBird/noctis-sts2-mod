using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Noctis.NoctisCode.Mechanics.Armiger;
using Noctis.NoctisCode.Powers;
using Noctis.NoctisCode.Relics;

namespace Noctis.NoctisCode.Extensions;

public static class NoctisExtensions
{
    public static async Task CheckArmigerReady(
        this Creature creature,
        PlayerChoiceContext? context,
        Creature source,
        CardModel? card)
    {
        var baseTurnAmount = 4;
        
        var player = creature.Player;

        if (player == null)
            return;

        // Already transformed
        if (creature.HasPower<ArmigerPower>())
            return;

        if (creature.HasPower<ArmigerUnleashedPower>())
            return;

        if (!ArmigerManager.IsFull(player))
            return;

        // Consume gauge
        // Ancient Relic version
        if (player.GetRelic<FoundersKingSigil>() != null)
        {
            baseTurnAmount += 1;
            await PowerCmd.Apply<ArmigerUnleashedPower>(
                context,
                creature,
                baseTurnAmount,
                creature,
                card
            );
        }
        else
        {
            await PowerCmd.Apply<ArmigerPower>(
                context,
                creature,
                baseTurnAmount,
                creature,
                card
            );
        }
    }
    
    public static class CombatHelpers
    {
        private const string DefaultHitVfxScene =
            "res://Noctis/scenes/vfx.tscn";

        private const string DefaultHitVfx = "hit";

        public const string NoHitVfx = "__NONE__";

        public static async Task NoctisFakeHit(
            Creature attacker,
            Creature target,
            string? attackSfx = null,
            string? hitSfx = null,
            string? hitVfx = null,
            bool playHitVfx = true)
        {
            if (target == null)
                return;

            if (attackSfx != null)
                SfxCmd.Play(attackSfx);

            if (hitSfx != null)
                SfxCmd.Play(hitSfx);

            var noctis = attacker.Player.Character as Character.Noctis;

            if (playHitVfx)
            {
                noctis.PlayVfxOnTarget(
                    target,
                    DefaultHitVfxScene,
                    hitVfx ?? DefaultHitVfx);
            }

            await CreatureCmd.TriggerAnim(target, "Hit", 0f);

            if (target.Monster?.HasHurtSfx == true)
                SfxCmd.Play(target.Monster.HurtSfx);
        }
    }
}