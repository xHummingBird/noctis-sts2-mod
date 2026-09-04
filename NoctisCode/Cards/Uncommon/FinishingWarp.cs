using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class FinishingWarp() : NoctisCard(3, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy), IWarpStrikeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(15m, ValueProp.Move),
        new EnergyVar(1)
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
            await noctis.DashTo(ownerCreature, play.Target, distance: 300f, durationSeconds: 0.001f, overrideAnim: "warp_strike");
            await Task.Delay((int)(0.199f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.067f * 1000f));
        }
        CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null)
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
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        if (base.IsClone)
        {
            return Task.CompletedTask;
        }
        int amount = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == base.Owner && e.HappenedThisTurn(base.CombatState));
        ReduceCostBy(amount);
        return Task.CompletedTask;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }
        ReduceCostBy(1);
        return Task.CompletedTask;
    }

    private void ReduceCostBy(int amount)
    {
        base.EnergyCost.AddThisTurn(-amount);
    }
}