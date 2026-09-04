using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Uncommon;

public class RoyalRecovery() : NoctisCard(0, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal => LostHpThisTurn(base.Owner.Creature);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(4m, ValueProp.Move),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        await CommonActions.CardBlock(this, cardPlay);
        if (LostHpThisTurn(Owner.Creature))
            await CommonActions.CardBlock(this, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(2m);
    }

    private static bool LostHpThisTurn(Creature creature)
    {
        return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any((DamageReceivedEntry e) => e.HappenedThisTurn(creature.CombatState) && e.Receiver == creature && e.Result.UnblockedDamage > 0);
    }
}