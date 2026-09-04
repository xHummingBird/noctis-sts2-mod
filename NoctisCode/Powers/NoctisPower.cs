using BaseLib.Abstracts;
using BaseLib.Extensions;
using Noctis.NoctisCode.Extensions;
using Godot;

namespace Noctis.NoctisCode.Powers;

public abstract class NoctisPower : CustomPowerModel
{
    //Loads from Noctis/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}