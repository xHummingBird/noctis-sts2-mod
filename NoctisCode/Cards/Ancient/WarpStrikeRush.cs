using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Mechanics.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Ancient;

public class WarpStrikeRush() : NoctisCard(1, CardType.Attack,
    CardRarity.Ancient, TargetType.AnyEnemy), IWarpStrikeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(18, ValueProp.Move),
        new DynamicVar("TransformTurns", 4)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.WarpStrike,
        HoverTipFactory.FromPower<ArmigerUnleashedPower>()
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var transformTurns = DynamicVars["TransformTurns"].IntValue;
        bool armiger = Owner.Creature.HasPower<ArmigerPower>();
        if (armiger)
        {
            await PowerCmd.Remove<ArmigerPower>(Owner.Creature);
        }
        ArmigerManager.SetArmiger(Owner, 100);
        await PowerCmd.Apply<ArmigerUnleashedPower>(choiceContext, Owner.Creature, transformTurns, Owner.Creature,
            this);

        var ownerCreature = Owner?.Creature;
        var noctis = Owner?.Character as Character.Noctis;
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        if (ownerCreature != null && noctis != null)
        {
            AudioHelper.PlayRandomLimitBreak();
            float duration = noctis.PlayAnimation(ownerCreature, "armiger_chain").total;
            SfxCmd.Play("res://Noctis/sfx/warp.wav");
            await Task.Delay((int)(0.234f * 1000f));
            await noctis.DashTo(ownerCreature, play.Target, distance: 260f, durationSeconds: 0.066f,
                overrideAnim: "armiger_chain");
            await Task.Delay((int)(0.067f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/shift_attack.wav");
            await Task.Delay((int)(0.066f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target,
                "res://Noctis/sfx/enemy_hit_slash (2).wav");
            await Task.Delay((int)(0.300f * 1000f));
            AudioHelper.PlayRandomAttackMedium();
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.033f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target);
            await Task.Delay((int)(0.300f * 1000f));
            AudioHelper.PlayRandomAttackMedium();
            SfxCmd.Play("res://Noctis/sfx/slash (2).wav");
            await Task.Delay((int)(0.034f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target);
            await Task.Delay((int)(0.267f * 1000f));
            AudioHelper.PlayRandomAttackMedium();
            SfxCmd.Play("res://Noctis/sfx/slash (4).wav");
            await Task.Delay((int)(0.066f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target);
            await Task.Delay((int)(0.333f * 1000f));
            AudioHelper.PlayRandomLastHit();
            await Task.Delay((int)(0.200f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/shift_attack.wav");
            await Task.Delay((int)(0.067f * 1000f));
        }

        await CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null, "res://Noctis/sfx/enemy_hit_slash (2).wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.667f * 1000f));
        if (ownerCreature != null && noctis != null)
        {
            await noctis.Retreat(ownerCreature, null, false, 0.1f);
            noctis.PlayAnimation(ownerCreature, "retreat_unleashed_2");
            await Task.Delay((int)(0.3f * 1000f));
        }

        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}