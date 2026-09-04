using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Noctis.NoctisCode.Powers;

public class RingsBlessingPower : NoctisPower
{
    private const string _selfDamageKey = "SelfDamage";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered)
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            Flash();
            DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
            await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner);
            await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null);
        }
    }

    public void IncrementSelfDamage()
    {
        AssertMutable();
        base.DynamicVars["SelfDamage"].BaseValue++;
    }
}