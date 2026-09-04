using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Noctis.NoctisCode.Extensions;

public static class NoctisStaticHoverTip
{
    public static readonly IHoverTip WarpStrike = new HoverTip(
        new LocString("static_hover_tips", "NOCTIS_WARP_STRIKE.title"),
        new LocString("static_hover_tips", "NOCTIS_WARP_STRIKE.description")
    );

    public static readonly IHoverTip Armiger = new HoverTip(
        new LocString("static_hover_tips", "NOCTIS_ARMIGER.title"),
        new LocString("static_hover_tips", "NOCTIS_ARMIGER.description")
    );

    public static readonly IHoverTip RoyalArms = new HoverTip(
        new LocString("static_hover_tips", "NOCTIS_ROYAL_ARMS.title"),
        new LocString("static_hover_tips", "NOCTIS_ROYAL_ARMS.description")
    );
}