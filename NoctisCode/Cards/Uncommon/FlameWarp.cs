using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class FlameWarp() : NoctisCard(1, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy), IWarpStrikeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9, ValueProp.Move),
        new PowerVar<VulnerablePower>(1m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.WarpStrike,
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        bool armiger = Owner.Creature.HasPower<ArmigerPower>();

        var ownerCreature = Owner?.Creature;
        var noctis = Owner?.Character as Character.Noctis;
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        if (ownerCreature != null && noctis != null)
        {
            AudioHelper.PlayRandomPhrase();
            float duration = noctis.PlayAnimation(ownerCreature, "warp_strike").total;
            if (Owner.Creature.HasPower<ArmigerPower>())
                await Task.Delay((int)(0.067f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/warp.wav");
            await Task.Delay((int)(0.2f * 1000f));
            await noctis.DashTo(ownerCreature, play.Target, distance: 300f, durationSeconds: 0.001f, overrideAnim: "warp_strike");
            await Task.Delay((int)(0.199f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.067f * 1000f));
        }

        CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null)
            .BeforeDamage(async delegate
            {
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(play.Target));
                SfxCmd.Play("event:/sfx/characters/attack_fire");
            })
            .Execute(choiceContext);
        await Task.Delay((int)(0.3f * 1000f));
        if (ownerCreature != null && noctis != null)
        {
            if (base.Owner.HasPower<ArmigerPower>())
            {
                await noctis.Retreat(ownerCreature, "warp_strike", false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_armiger_2");
                await Task.Delay((int)(0.233f * 1000f));
            }
            else if (base.Owner.HasPower<ArmigerUnleashedPower>())
            {
                await noctis.Retreat(ownerCreature, "warp_strike", false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_unleashed");
                await Task.Delay((int)(0.3f * 1000f));
            }
            else
                await noctis.Retreat(ownerCreature);
        }
        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}