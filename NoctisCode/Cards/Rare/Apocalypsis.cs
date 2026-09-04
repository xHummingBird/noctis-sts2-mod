using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Rare;

public class Apocalypsis() : NoctisCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private static readonly string[] HitSounds =
    [
        "res://Noctis/sfx/enemy_hit_slash (1).wav",
        "res://Noctis/sfx/enemy_hit_slash (2).wav",
        "res://Noctis/sfx/enemy_hit_slash (3).wav",
        "res://Noctis/sfx/enemy_hit_slash (4).wav",
        "res://Noctis/sfx/enemy_hit_slash (1).wav",
        "res://Noctis/sfx/enemy_hit_slash (2).wav",
        "res://Noctis/sfx/enemy_hit_slash (3).wav",
        "res://Noctis/sfx/enemy_hit_slash (4).wav",
        "res://Noctis/sfx/enemy_hit_slash (1).wav"
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(0m),
        new ExtraDamageVar(6m),
        new CalculatedDamageVar(ValueProp.Move)
            .WithMultiplier((card, _) =>
                CombatManager.Instance.History.CardPlaysFinished.Count(
                    entry =>
                        entry.CardPlay.Card.Owner == card.Owner &&
                        entry.CardPlay.Card is IRoyalArmsCard))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        var ownerCreature = Owner.Creature;
        var noctis = Owner.Character as Character.Noctis;

        if (noctis == null)
        {
            await CommonActions.CardAttack(this, cardPlay.Target)
                .WithHitFx(
                    null,
                    "res://Noctis/sfx/enemy_hit_slash (3).wav")
                .Execute(choiceContext);

            return;
        }

        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        AudioHelper.PlayRandomLastHit2();
        SfxCmd.Play("res://Noctis/sfx/armiger_open.wav");

        string animation = ownerCreature.HasPower<ArmigerUnleashedPower>() ? "apocalypsis_unleashed" : "apocalypsis";

        noctis.PlayAnimation(ownerCreature, animation);

        await Task.Delay(167);

        await noctis.DashTo(ownerCreature, cardPlay.Target, distance: 600f, durationSeconds: 0.033f, overrideAnim: animation);

        await Task.Delay(1000);

        foreach (string hitSound in HitSounds)
        {
            NoctisExtensions.CombatHelpers.NoctisFakeHit(ownerCreature, cardPlay.Target, hitSound);
            await Task.Delay(200);
        }

        noctis.PlayVfxOnTarget(cardPlay.Target, "res://Noctis/scenes/vfx.tscn", "hit");

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .WithHitFx(null, "res://Noctis/sfx/enemy_hit_slash (2).wav")
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await Task.Delay(433);

        await noctis.Retreat(ownerCreature, null, duration: 0.2f);

        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(2m);
    }
}