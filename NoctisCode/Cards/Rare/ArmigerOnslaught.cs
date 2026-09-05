using BaseLib.Extensions;
using BaseLib.Utils;
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

public class ArmigerOnslaught() : NoctisCard(2, CardType.Attack,
    CardRarity.Rare, TargetType.AllEnemies)
{
    protected override bool ShouldGlowGoldInternal =>
        PileType.Hand.GetPile(Owner).Cards.Count(c => c is IRoyalArmsCard) >= 1;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(12m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => PileType.Hand.GetPile(card.Owner).Cards.Count(c => c is IRoyalArmsCard))
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var royalArms = PileType.Hand
            .GetPile(Owner)
            .Cards
            .Where(c => c is IRoyalArmsCard)
            .ToList();
        var multiplier = royalArms.Count;
        
        var damage = DynamicVars.Damage.BaseValue * multiplier;
        
        foreach (var card in royalArms)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
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
                noctis.PlayAnimation(ownerCreature, "armiger_barrage_unleashed");
            else
                noctis.PlayAnimation(ownerCreature, "armiger_barrage");

            SfxCmd.Play("res://Noctis/sfx/armiger_open.wav");

            await Task.Delay(1000);
            
            SfxCmd.Play("res://Noctis/sfx/royal_cleave.wav");
            
            await Task.Delay(133);
            
            SfxCmd.Play("res://Noctis/sfx/armiger_close.wav");
            
            await Task.Delay(133);
            await DamageCmd.Attack(damage).FromCard(this, play).TargetingAllOpponents(base.CombatState)
                .WithValueProp(ValueProp.Move)
                .Execute(choiceContext);

            await Task.Delay(933);
            CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        }
        
    }
    
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}