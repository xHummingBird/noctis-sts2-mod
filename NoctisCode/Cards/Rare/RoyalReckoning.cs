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
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Rare;

public class RoyalReckoning() : NoctisCard(2, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy), IWarpStrikeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(4m),
        new ExtraDamageVar(5m),
        new CalculatedDamageVar(ValueProp.Move)
            .WithMultiplier(
                (card, _) =>
                    PileType.Exhaust
                        .GetPile(card.Owner)
                        .Cards
                        .Count(c => c is IWarpStrikeCard)
            )
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
        bool unleashed = Owner.Creature.HasPower<ArmigerUnleashedPower>();

        var ownerCreature = Owner?.Creature;
        var noctis = Owner?.Character as Character.Noctis;
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        if (ownerCreature != null && noctis != null)
        {
            AudioHelper.PlayRandomPhrase();
            float duration = noctis.PlayAnimation(ownerCreature, "triple_warp").total;
            if (armiger)
                await Task.Delay((int)(0.067f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/warp.wav");
            if (unleashed)
                await Task.Delay((int)(0.067f * 1000f));
            await Task.Delay((int)(0.2f * 1000f));
            await noctis.DashTo(ownerCreature, play.Target, distance: 300f, durationSeconds: 0.001f,
                overrideAnim: "triple_warp");
            if (!unleashed)
            {
                await Task.Delay((int)(0.199f * 1000f));
            }
            else await Task.Delay((int)(0.099f * 1000f));

            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            if (!unleashed)
                await Task.Delay((int)(0.067f * 1000f));
            else await Task.Delay((int)(0.033f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(ownerCreature, play.Target, null, null, playHitVfx: false);
            if (!unleashed)
            {
                await Task.Delay((int)(0.133f * 1000f));
                if (armiger)
                    await Task.Delay((int)(0.034f * 1000f));
            }
            else await Task.Delay((int)(0.467f * 1000f));

            SfxCmd.Play("res://Noctis/sfx/slash (2).wav");
            if (!unleashed)
                await Task.Delay((int)(0.067f * 1000f));
            else await Task.Delay((int)(0.033f * 1000f));
            NoctisExtensions.CombatHelpers.NoctisFakeHit(ownerCreature, play.Target, null, null, playHitVfx: false);
            await Task.Delay((int)(0.200f * 1000f));
            if (armiger)
                await Task.Delay((int)(0.133f * 1000f));
            if (unleashed)
                await Task.Delay((int)(0.500f * 1000f));
        }

        CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null, "res://Noctis/sfx/slash (3).wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.267f * 1000f));
        if (ownerCreature != null && noctis != null)
        {
            if (armiger)
            {
                await Task.Delay((int)(0.033f * 1000f));
                SfxCmd.Play("res://Noctis/sfx/weapon_disappear_1.wav");
                await Task.Delay((int)(0.567f * 1000f));
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_armiger_2");
                await Task.Delay((int)(0.233f * 1000f));
            }
            else if (unleashed)
            {
                await Task.Delay((int)(0.233f * 1000f));
                await noctis.Retreat(ownerCreature, null, false, 0.1f);
                noctis.PlayAnimation(ownerCreature, "retreat_unleashed");
                await Task.Delay((int)(0.3f * 1000f));
            }
            else
                await noctis.Retreat(ownerCreature);
        }

        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(2);
    }
}