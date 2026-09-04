using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;

namespace Noctis.NoctisCode.Cards.Rare;

public class Alterna() : NoctisCard (0, CardType.Attack,
    CardRarity.Rare, TargetType.AllEnemies)
{
    protected override bool HasEnergyCostX => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9m, ValueProp.Move),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var enemies = base.CombatState.HittableEnemies.ToList();

        var target =
            enemies[(enemies.Count - 1) / 2];
        
        int num = ResolveEnergyXValue();
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        AudioHelper.PlayRandomLimitBreak();
        var ownerCreature = Owner?.Creature;
        if (ownerCreature != null && Owner?.Character is Character.Noctis noctis)
        {
            noctis.PlayVfxOnTarget(
                target,
                "res://Noctis/scenes/vfx.tscn",
                "dark_messenger"
            );
            SfxCmd.Play("res://Noctis/sfx/dark_messenger_sfx_1.wav");
            await Task.Delay((int)(0.9f * 1000f));
            SfxCmd.Play("res://Noctis/sfx/dark_messenger_vfx_2.wav");
        }
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this, play)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);
        await Task.Delay((int)(0.8f * 1000f));
        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}