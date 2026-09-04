using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Common;

public class Regroup() : NoctisCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AudioHelper.PlayRandomDefend();
        await CommonActions.CardBlock(this, play);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}