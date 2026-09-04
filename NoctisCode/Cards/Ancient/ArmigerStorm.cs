using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Ancient;

public class ArmigerStorm() : NoctisCard(0, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(30, ValueProp.Move),
        new PowerVar<VulnerablePower>(2m),
        new PowerVar<WeakPower>(2m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var enemies = CombatState.HittableEnemies.ToList();
        var ownerCreature = Owner?.Creature;
        var noctis = Owner?.Character as Character.Noctis;
        
        if (noctis == null)
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(
                    null,
                    "res://Noctis/sfx/enemy_hit_slash (3).wav"
                )
                .Execute(choiceContext);
        else
        {
            CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
            AudioHelper.PlayRandomLastHit2();

            if (ownerCreature.HasPower<ArmigerUnleashedPower>())
                noctis.PlayAnimation(ownerCreature, "armiger_storm_unleashed");
            else
                noctis.PlayAnimation(ownerCreature, "armiger_storm");

            SfxCmd.Play("res://Noctis/sfx/armiger_open.wav");

            await Task.Delay(1033);

            string[] hitSounds =
            {
                "res://Noctis/sfx/enemy_hit_slash (1).wav",
                "res://Noctis/sfx/enemy_hit_slash (2).wav",
                "res://Noctis/sfx/enemy_hit_slash (3).wav",
                "res://Noctis/sfx/enemy_hit_slash (4).wav",
                "res://Noctis/sfx/enemy_hit_slash (1).wav",
                "res://Noctis/sfx/enemy_hit_slash (2).wav",
                "res://Noctis/sfx/enemy_hit_slash (3).wav",
                "res://Noctis/sfx/enemy_hit_slash (4).wav",
                "res://Noctis/sfx/enemy_hit_slash (1).wav",
                "res://Noctis/sfx/enemy_hit_slash (2).wav"
            };

            foreach (var hitSound in hitSounds)
            {
                foreach (var enemy in enemies)
                {
                    NoctisExtensions.CombatHelpers.NoctisFakeHit(
                        ownerCreature,
                        enemy,
                        null
                    );
                }

                // Played once per wave, not once per enemy.
                SfxCmd.Play(hitSound);

                await Task.Delay(100);
            }

            foreach (var enemy in enemies)
            {
                noctis.PlayVfxOnTarget(
                    enemy,
                    "res://Noctis/scenes/vfx.tscn",
                    "hit"
                );
            }

            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(
                    null,
                    "res://Noctis/sfx/enemy_hit_slash (3).wav"
                )
                .Execute(choiceContext);

            await Task.Delay(833);
            CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        }

        await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Weak.BaseValue, Owner.Creature, this);
        await PowerCmd.Remove<ArmigerArtsReadyPower>(ownerCreature);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8);
        DynamicVars.Weak.UpgradeValueBy(1);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}