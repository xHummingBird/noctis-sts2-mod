using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Ancient;

public class ArmigerBarrage() : NoctisCard(0, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(12m),
        new ExtraDamageVar(3m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.PlayerCombatState.AllCards.Count((CardModel c) => c is IRoyalArmsCard))
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
                noctis.PlayAnimation(ownerCreature, "armiger_onslaught_unleashed");
            else
                noctis.PlayAnimation(ownerCreature, "armiger_onslaught");

            SfxCmd.Play("res://Noctis/sfx/armiger_open.wav");

            await Task.Delay(667);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (1).wav");
            
            await Task.Delay(200);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (2).wav");
            
            await Task.Delay(133);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (3).wav");
            
            await Task.Delay(100);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (4).wav");
            
            await Task.Delay(100);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (1).wav");
            
            await Task.Delay(200);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (2).wav");
            
            await Task.Delay(300);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (3).wav");
            
            await Task.Delay(200);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (4).wav");
            
            await Task.Delay(300);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (1).wav");
            
            await Task.Delay(100);
            foreach (var enemy in enemies)
            {
                NoctisExtensions.CombatHelpers.NoctisFakeHit(
                    ownerCreature,
                    enemy,
                    null
                );
            }
            SfxCmd.Play("res://Noctis/sfx/enemy_hit_slash (2).wav");
            
            await Task.Delay(67);
            SfxCmd.Play("res://Noctis/sfx/weapon_appear_1.wav");
            
            await Task.Delay(533);
            SfxCmd.Play("res://Noctis/sfx/legacy_of_lucii_final_3.wav");
            
            await Task.Delay(400);
            await CommonActions.CardAttack(this, play.Target)
                .Execute(choiceContext);

            await Task.Delay(1233);
            CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        }
        await PowerCmd.Remove<ArmigerArtsReadyPower>(ownerCreature);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(4);
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}