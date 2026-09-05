using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Relics;

public sealed class FoundersKingSigil : ArmigerRelicBase
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ArmigerUnleashedPower>(),
    ];
    
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override int BaseTurnArmigerGain => 6;
    protected override int BaseAttackArmigerGain => 6;

    protected override int WarpStrikeArmigerGain => 9;
    protected override int RoyalArmsArmigerGain => 9;

    protected override int EndCombatHealPercent => 15;
}