using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Noctis.NoctisCode.Character;
using Noctis.NoctisCode.Extensions;
using Godot;

namespace Noctis.NoctisCode.Relics;

[Pool(typeof(NoctisRelicPool))]
public abstract class NoctisRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".BigRelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}