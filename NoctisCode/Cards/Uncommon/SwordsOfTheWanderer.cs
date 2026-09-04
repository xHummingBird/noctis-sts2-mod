using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class SwordsOfTheWanderer() : NoctisCard(0, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy), IRoyalArmsCard
{
    protected override bool ShouldGlowGoldInternal => base.Owner.HasPower<ArmigerPower>() || base.Owner.HasPower<ArmigerUnleashedPower>();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.RoyalArms,
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3, ValueProp.Move),
        new RepeatVar(2),
        new CardsVar(1),
        new EnergyVar(1)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        if (!Owner.Creature.HasPower<ArmigerPower>() && !Owner.Creature.HasPower<ArmigerUnleashedPower>())
            await CreatureCmd.Damage(choiceContext, base.Owner.Creature, 2, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, play);
        if (ownerCreature != null && Owner?.Character is Character.Noctis noctis)
        {
            AudioHelper.PlayRandomPhrase();
            float duration = noctis.PlayAnimation(ownerCreature, "attack").total;
           
            await Task.Delay((int)(0.133f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.067f * 1000f));
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .WithValueProp(ValueProp.Move)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        if (Owner.Creature.HasPower<ArmigerPower>() || Owner.Creature.HasPower<ArmigerUnleashedPower>())
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}