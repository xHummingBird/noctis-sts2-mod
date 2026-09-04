using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Mechanics.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Ancient;

//deal 70 damage to 1 enemy. End armiger/armiger unleashed.
public class LegacyOfTheLucii() : NoctisCard(0, CardType.Attack,
    CardRarity.Ancient, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(75, ValueProp.Move),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        var noctis = Owner?.Character as Character.Noctis;
        await PowerCmd.Remove<ArmigerArtsReadyPower>(ownerCreature);
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        if (ownerCreature != null && noctis != null)
        {
            SfxCmd.Play("res://Noctis/sounds/run_start.wav");
            float duration = noctis.PlayAnimation(ownerCreature, "warp_strike").total;
            if (Owner.Creature.HasPower<ArmigerPower>())
                await Task.Delay((int)(0.067f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/warp.wav");
            await Task.Delay((int)(0.2f * 1000f));
            await noctis.DashTo(ownerCreature, play.Target, distance: 300f, durationSeconds: 0.001f,
                overrideAnim: "warp_strike");
            await Task.Delay((int)(0.099f * 1000f));
            if (base.Owner.HasPower<ArmigerUnleashedPower>())
                noctis.PlayAnimation(ownerCreature, "legacy_of_lucii_unleashed");
            else noctis.PlayAnimation(ownerCreature, "legacy_of_lucii_armiger");
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target,
                "res://Noctis/sfx/shift_attack.wav");
            await Task.Delay((int)(0.267f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target,
                "res://Noctis/sfx/slash (5).wav");
            await Task.Delay((int)(0.066f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target,
                "res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.134f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (2).wav");
            await Task.Delay((int)(0.066f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target);
            await Task.Delay((int)(0.134f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target,
                "res://Noctis/sfx/slash (3).wav");
            await Task.Delay((int)(0.066f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (4).wav");
            await Task.Delay((int)(0.067f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target);
            await Task.Delay((int)(0.300f * 1000f));
            await Task.Delay((int)(0.033f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(Owner.Creature, play.Target,
                "res://Noctis/sfx/slash (5).wav");
            await Task.Delay((int)(0.267f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/legacy_of_lucii_final_1.wav");
            await Task.Delay((int)(0.400f * 1000f));
            AudioHelper.PlayRandomLastHit2();
            await Task.Delay((int)(0.133f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/legacy_of_lucii_final_2.wav");
            await Task.Delay((int)(0.034f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/legacy_of_lucii_final_3.wav");
            await Task.Delay((int)(0.233f * 1000f));
            await CommonActions.CardAttack(this, play.Target)
                .Execute(choiceContext);
            if (Owner.HasPower<ArmigerPower>())
            {
                ArmigerManager.Reset(Owner);
                await Task.Delay((int)(1.800f * 1000f));
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_armiger_2");
                await PowerCmd.Remove<ArmigerPower>(ownerCreature);
                await Task.Delay((int)(0.223f * 1000f));
            }
            else if (Owner.HasPower<ArmigerUnleashedPower>())
            {
                ArmigerManager.Reset(Owner);
                await Task.Delay((int)(1.667f * 1000f));
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_unleashed_2");
                await PowerCmd.Remove<ArmigerUnleashedPower>(ownerCreature);
                await Task.Delay((int)(0.3f * 1000f));
            }
            ArmigerManager.SetArmiger(Owner, 0);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(15);
    }
}