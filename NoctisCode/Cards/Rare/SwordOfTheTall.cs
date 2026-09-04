using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Cards.Ancient;
using Noctis.NoctisCode.Cards.Rare;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Powers;
using Noctis.NoctisCode.Relics;

namespace Noctis.NoctisCode.Cards.Rare;

public class SwordOfTheTall() : NoctisCard(2, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy), IRoyalArmsCard
{
    protected override bool ShouldGlowGoldInternal => base.Owner.HasPower<ArmigerArtsReadyPower>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(25, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ArmigerArtsReadyPower>(),
        NoctisStaticHoverTip.RoyalArms,
        HoverTipFactory.FromCard<RoyalCleave>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        if (!Owner.Creature.HasPower<ArmigerPower>() && !Owner.Creature.HasPower<ArmigerUnleashedPower>())
            await CreatureCmd.Damage(choiceContext, base.Owner.Creature, 3, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, play);
        if (Owner.Creature.HasPower<ArmigerArtsReadyPower>())
        {
            CardModel card = CombatState.CreateCard<RoyalCleave>(base.Owner);
            RoyalSigil? royalSigil = Owner.GetRelic<RoyalSigil>();
            if (royalSigil != null)
                CardCmd.Upgrade(card);
            await CardCmd.AutoPlay(choiceContext, card, null);
            return;
        }
        if (ownerCreature != null && Owner?.Character is Character.Noctis noctis)
        {
            AudioHelper.PlayRandomPhrase();
            float duration = noctis.PlayAnimation(ownerCreature, "attack").total;
           
            await Task.Delay((int)(0.133f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.067f * 1000f));
        }
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithValueProp(ValueProp.Unpowered)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(7);
    }
}