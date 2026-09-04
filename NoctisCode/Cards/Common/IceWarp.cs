using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Common;

public class IceWarp() : NoctisCard(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy), IWarpStrikeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move),
        new BlockVar(5, ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.WarpStrike,
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
            await noctis.DashTo(ownerCreature, play.Target, distance: 300f, durationSeconds: 0.02f, overrideAnim: "warp_strike");
            await Task.Delay((int)(0.047f * 1000f));
            await Task.Delay((int)(0.133f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.067f * 1000f));
            noctis.PlayVfxOnTarget(
                play.Target,
                "res://Noctis/scenes/vfx.tscn",
                "ice_1"
            );
        }

        CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null, "res://Noctis/sfx/ice.wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.3f * 1000f));
        if (ownerCreature != null && noctis != null)
        {
            if (base.Owner.HasPower<ArmigerPower>())
            {
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_armiger_2");
                await Task.Delay((int)(0.233f * 1000f));
            }
            else if (base.Owner.HasPower<ArmigerUnleashedPower>())
            {
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_unleashed");
                await Task.Delay((int)(0.3f * 1000f));
            }
            else
                await noctis.Retreat(ownerCreature);
        }
        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Block.UpgradeValueBy(2);
    }
}