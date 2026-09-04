using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Noctis.NoctisCode.Powers;

//Warp Strike hits everyone. Upgrade all WarpStrikes generated
public class AwakeningPower : NoctisPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
}