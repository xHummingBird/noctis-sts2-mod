using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Cards.Common;

// 0 cost, 1 hp cost. Apply 2 weak. Upgraded is Innate. Armiger is Apply to all enemies
public class BowOfTheClever() : NoctisCard(0, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy), IRoyalArmsCard
{
    protected override bool ShouldGlowGoldInternal => base.Owner.HasPower<ArmigerPower>() || base.Owner.HasPower<ArmigerUnleashedPower>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WeakPower>(2)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.RoyalArms,
        HoverTipFactory.FromPower<WeakPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        if (ownerCreature != null && Owner?.Character is Character.Noctis noctis)
        {
            AudioHelper.PlayRandomAttack();
        }

        if (!Owner.Creature.HasPower<ArmigerPower>() && !Owner.Creature.HasPower<ArmigerUnleashedPower>())
        {
            await CreatureCmd.Damage(choiceContext, base.Owner.Creature, 1,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, play);
            await PowerCmd.Apply<WeakPower>(choiceContext, play.Target,
                base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
        }
        else 
            await PowerCmd.Apply<WeakPower>(choiceContext, base.CombatState?.HittableEnemies, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}