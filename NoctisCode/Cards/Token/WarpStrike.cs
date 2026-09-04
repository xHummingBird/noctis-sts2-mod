using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Token;

public class WarpStrike() : NoctisCard(0, CardType.Attack,
    CardRarity.Token, TargetType.AnyEnemy), IWarpStrikeCard
{
    public override bool CanBeGeneratedInCombat => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Move),
        new PowerVar<VulnerablePower>(1)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.WarpStrike,
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        bool armiger = Owner.Creature.HasPower<ArmigerPower>();
        
        await using AttackContext context =
            await AttackCommand.CreateContextAsync(
                base.CombatState,
                choiceContext,
                play);
        
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
        List<DamageResult> results =
        (
            await CreatureCmd.Damage(
                choiceContext,
                play.Target,
                base.DynamicVars.Damage.BaseValue,
                ValueProp.Move,
                this,
                play)
        ).ToList();

        context.AddHit(results);

        DamageResult? damageResult = results.FirstOrDefault();

        if (damageResult == null)
            return;

        if (base.Owner.HasPower<AwakeningPower>())
        {
            List<Creature> otherEnemies = base.CombatState
                .GetTeammatesOf(damageResult.Receiver)
                .Where(e =>
                    e != damageResult.Receiver &&
                    e.IsHittable)
                .ToList();

            if (otherEnemies.Count > 0)
            {
                context.AddHit(
                    await CreatureCmd.Damage(
                        choiceContext,
                        otherEnemies,
                        damageResult.TotalDamage,
                        ValueProp.Unpowered | ValueProp.Move,
                        ownerCreature,
                        this,
                        play)
                );
            }
        }
        await Task.Delay(300);
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
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
    
    public static async Task<CardModel?> CreateInHand(Player owner, ICombatState combatState, Player? creator = null)
    {
        return (await CreateInHand(owner, 1, combatState, creator)).FirstOrDefault();
    }

    public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, ICombatState combatState, Player? creator = null)
    {
        if (count == 0)
        {
            return Array.Empty<CardModel>();
        }
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return Array.Empty<CardModel>();
        }
        
        var warpStrike = combatState.CreateCard<WarpStrike>(owner);
        List<CardModel> warpStrikes = new List<CardModel>();
        for (int i = 0; i < count; i++)
        {
            if (owner.HasPower<AwakeningPower>())
                CardCmd.Upgrade(warpStrike);
            warpStrikes.Add(warpStrike);
            
        }
        await CardPileCmd.AddGeneratedCardsToCombat(warpStrikes, PileType.Hand, creator ?? owner);
        return warpStrikes;
    }
}