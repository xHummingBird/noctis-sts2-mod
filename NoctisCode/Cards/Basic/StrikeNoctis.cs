using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Basic;

public class StrikeNoctis() : NoctisCard(1, CardType.Attack,
    CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
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
        await CommonActions.CardAttack(this, play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}