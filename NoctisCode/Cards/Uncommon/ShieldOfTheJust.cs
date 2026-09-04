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

namespace Noctis.NoctisCode.Cards.Uncommon;

//Lose 2 HP, gain 16 block (gain 20 block upgraded), Armiger is gain same block next turn
public class ShieldOfTheJust() : NoctisCard(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self), IRoyalArmsCard
{
    protected override bool ShouldGlowGoldInternal => base.Owner.HasPower<ArmigerPower>() || base.Owner.HasPower<ArmigerUnleashedPower>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(16, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        NoctisStaticHoverTip.RoyalArms,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (!Owner.Creature.HasPower<ArmigerPower>() && !Owner.Creature.HasPower<ArmigerUnleashedPower>())
            await CreatureCmd.Damage(choiceContext, base.Owner.Creature, 2, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, play);
        AudioHelper.PlayRandomDefend();
        await CommonActions.CardBlock(this, play);
        if (Owner.Creature.HasPower<ArmigerPower>() || Owner.Creature.HasPower<ArmigerUnleashedPower>())
            await PowerCmd.Apply<BlockNextTurnPower>(
                choiceContext,
                Owner.Creature,
                DynamicVars.Block.BaseValue,
                Owner.Creature,
                this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(4m);
    }
}