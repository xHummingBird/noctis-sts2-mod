using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Cards.Common;

//Increase Armiger Charge
public class ArmigerCharge() : NoctisCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Armiger", 5),
        new BlockVar(5, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudioHelper.PlayRandomDefend();
        ArmigerManager.GainArmiger(Owner, DynamicVars["Armiger"].IntValue);
        await CommonActions.CardBlock(this, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Armiger"].UpgradeValueBy(3m);
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}