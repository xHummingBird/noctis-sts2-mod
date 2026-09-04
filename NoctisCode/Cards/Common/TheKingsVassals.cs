using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Common;

public class TheKingsVassals() : NoctisCard(2, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

        if (ownerCreature != null && Owner?.Character is Character.Noctis noctis)
        {
            AudioHelper.PlayRandomAttack();
            float duration = noctis.PlayAnimation(ownerCreature, "attack").total;
           
            await Task.Delay((int)(0.133f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/slash (1).wav");
            await Task.Delay((int)(0.067f * 1000f));
        }
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, play).WithHitCount(2)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        CardModel cardModel = PileType.Draw.GetPile(base.Owner).Cards.Where((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle)
            .FirstOrDefault();
        if (cardModel == null)
        {
            cardModel = PileType.Draw.GetPile(base.Owner).Cards.Where((CardModel c) => c.Type == CardType.Attack).ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle)
                .FirstOrDefault();
        }
        if (cardModel != null)
        {
            await CardCmd.AutoPlay(choiceContext, cardModel, null);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}