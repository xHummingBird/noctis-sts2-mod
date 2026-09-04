using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Rare;

public class Death() : NoctisCard (2, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20m, ValueProp.Move),
        new HealVar(15m)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        AudioHelper.PlayRandomLimitBreak();
        var ownerCreature = Owner?.Creature;
        decimal healAmount = DynamicVars.Damage.PreviewValue * (DynamicVars.Heal.BaseValue / 100m);
        if (ownerCreature != null && Owner?.Character is Character.Noctis noctis)
        {
                noctis.PlayVfxOnTarget(
                    play.Target,
                    "res://Noctis/scenes/vfx.tscn",
                    "death"
                );
            SfxCmd.Play("res://Noctis/sfx/dark_messenger_sfx_1.wav");
            await Task.Delay((int)(0.9f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/dark_messenger_vfx_2.wav");
        }
        await CommonActions.CardAttack(this, play.Target)
            .Execute(choiceContext);
        await Task.Delay((int)(1.0f * 1000f));
        await CreatureCmd.Heal(base.Owner.Creature, healAmount);
        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
        DynamicVars.Heal.UpgradeValueBy(3m);
    }
}