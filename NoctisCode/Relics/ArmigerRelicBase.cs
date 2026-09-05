using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanic.Armiger;
using Noctis.NoctisCode.Mechanic.WarpStrike;
using Noctis.NoctisCode.Mechanics.Armiger;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Relics;

public abstract class ArmigerRelicBase : NoctisRelic
{
    // ---------- Override values ----------
    protected virtual int BaseTurnArmigerGain => 4;
    protected virtual int BaseAttackArmigerGain => 4;
    protected virtual int WarpStrikeArmigerGain => 7;
    protected virtual int RoyalArmsArmigerGain => 7;
    
    protected virtual int EndCombatHealPercent => 10;

    [SavedProperty]
    public int StoredArmiger { get; set; }

    public override Task BeforeCombatStart()
    {
        ArmigerManager.Reset(Owner);

        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;

        if (card.Owner != Owner)
            return;

        if (ShouldBlockArmigerGain())
            return;

        int gain = GetCardArmigerGain(card);

        if (gain <= 0)
        {
            await Owner.Creature.CheckArmigerReady(
                choiceContext,
                Owner.Creature,
                card
            );
            return;
        }

        ArmigerManager.GainArmiger(Owner, gain);

        await Owner.Creature.CheckArmigerReady(
            choiceContext,
            Owner.Creature,
            card
        );
    }

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (ShouldBlockArmigerGain())
            return;

        SfxCmd.Play("event:/sfx/ui/relic_activate_general");
        
        ArmigerManager.GainArmiger(Owner, GetTurnArmigerGain());

        await Owner.Creature.CheckArmigerReady(
            null,
            Owner.Creature,
            null
        );
    }

    protected virtual int GetCardArmigerGain(CardModel card)
    {
        EngineBlade? engineBlade = Owner.GetRelic<EngineBlade>();
        // Important:
        // Warp Strike and Royal Arms are probably also Attack cards,
        // so handle them first to avoid giving them 6 + 3.
        if (card is IWarpStrikeCard)
        {
            if (engineBlade != null)
                return WarpStrikeArmigerGain;
            else return BaseAttackArmigerGain;
        }

        if (card is IRoyalArmsCard)
            return RoyalArmsArmigerGain + Owner.Creature.GetPowerAmount<ArmigerMasteryPower>();

        if (card.Type == CardType.Attack)
            return BaseAttackArmigerGain;

        return 0;
    }
    
    public override async Task AfterCombatVictory(CombatRoom _)
    {
        if (base.Owner.Creature.IsDead)
            return;

        int armiger = ArmigerManager.GetArmiger(base.Owner);
        
        int healAmount = armiger * EndCombatHealPercent / 100; // 10%
        
        if (Owner.HasPower<ArmigerPower>() || Owner.HasPower<ArmigerUnleashedPower>())
            healAmount = EndCombatHealPercent;

        if (healAmount > 0)
        {
            Flash();
            await CreatureCmd.Heal(base.Owner.Creature, healAmount);
        }

        ArmigerManager.Reset(base.Owner);
    }

    protected virtual int GetTurnArmigerGain()
    {
        return BaseTurnArmigerGain;
    }

    protected bool ShouldBlockArmigerGain()
    {
        if (Owner.Creature.HasPower<ArmigerPower>())
            return true;

        if (Owner.Creature.HasPower<ArmigerUnleashedPower>())
            return true;

        return false;
    }
    
}