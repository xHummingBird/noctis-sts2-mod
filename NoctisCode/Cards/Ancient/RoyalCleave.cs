using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Ancient;

public class RoyalCleave() : NoctisCard(0, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(50, ValueProp.Move),
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
        
        var leftMostEnemy = enemies[0];
        
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
            SfxCmd.Play("res://Noctis/sfx/weapon_appear_2.wav");
            if (ownerCreature.HasPower<ArmigerUnleashedPower>())
            {
                noctis.PlayAnimation(ownerCreature, "royal_cleave_unleashed");
                await Task.Delay((int)(0.167f * 1000f));
                await noctis.DashTo(ownerCreature, leftMostEnemy, distance: 430f, durationSeconds: 0.033f,
                    overrideAnim: "royal_cleave_unleashed");
            }
            else
            {
                noctis.PlayAnimation(ownerCreature, "royal_cleave");
                await Task.Delay((int)(0.167f * 1000f));
                await noctis.DashTo(ownerCreature, leftMostEnemy, distance: 430f, durationSeconds: 0.033f,
                    overrideAnim: "royal_cleave");
            }

            await Task.Delay((int)(0.533f * 1000f));
            foreach (var enemy in enemies)
            {
                noctis.PlayVfxOnTarget(
                    enemy,
                    "res://Noctis/scenes/vfx.tscn",
                    "hit"
                );
            }
            
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "res://Noctis/sfx/royal_cleave.wav")
                .Execute(choiceContext);

            await Task.Delay(833);
            if (Owner.HasPower<ArmigerPower>())
            {
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_armiger_2");
                await Task.Delay((int)(0.233f * 1000f));
            }
            else if (Owner.HasPower<ArmigerUnleashedPower>())
            {
                await Task.Delay((int)(1.667f * 1000f));
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_unleashed_2");
                await Task.Delay((int)(0.3f * 1000f));
            }
            CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        }
        await PowerCmd.Remove<ArmigerArtsReadyPower>(ownerCreature);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(10);
    }
}