using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Relics;

public sealed class RingOfTheLucii : ArmigerRelicBase
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ArmigerPower>(),
    ];

    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override int BaseTurnArmigerGain => 4;
    protected override int BaseAttackArmigerGain => 4;

    protected override int WarpStrikeArmigerGain => 7;
    protected override int RoyalArmsArmigerGain => 7;

    protected override int EndCombatHealPercent => 10;
}