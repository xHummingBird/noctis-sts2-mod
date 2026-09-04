using Noctis.NoctisCode.Relics;
using MegaCrit.Sts2.Core.Entities.Players;
using Noctis.NoctisCode.Powers;

namespace Noctis.NoctisCode.Mechanics.Armiger;

public static class ArmigerManager
{
    public class ArmigerData
    {
        public Action<int>? OnArmigerChanged;
    }

    private const int MaxArmiger = 100;

    private static readonly Dictionary<Player, ArmigerData> _data = new();

    private static ArmigerRelicBase? GetRelic(Player player)
    {
        return player.Relics
            .OfType<ArmigerRelicBase>()
            .FirstOrDefault();
    }
    
    public static bool CanGainArmiger(Player player)
    {
        var creature = player.Creature;

        if (creature.HasPower<ArmigerPower>())
            return false;

        if (creature.HasPower<ArmigerUnleashedPower>())
            return false;

        return true;
    }

    private static ArmigerData GetData(Player player)
    {
        if (!_data.TryGetValue(player, out var data))
        {
            data = new ArmigerData();
            _data[player] = data;
        }

        return data;
    }

    public static int GetArmiger(Player player)
    {
        return GetRelic(player)?.StoredArmiger ?? 0;
    }

    public static void SetArmiger(Player player, int value)
    {
        var relic = GetRelic(player);

        if (relic == null)
            return;

        value = Math.Clamp(value, 0, MaxArmiger);

        if (relic.StoredArmiger == value)
            return;

        relic.StoredArmiger = value;

        GetData(player).OnArmigerChanged?.Invoke(value);
    }

    public static void HalfArmiger(Player player)
    {
        int current = GetArmiger(player);

        if (current <= 0)
            return;

        SetArmiger(player, current / 2);
    }

    public static void GainArmiger(Player player, int amount)
    {
        if (!CanGainArmiger(player))
            return;

        SetArmiger(player, GetArmiger(player) + amount);
    }

    public static void SpendArmiger(Player player, int amount)
    {
        SetArmiger(player, GetArmiger(player) - amount);
    }

    public static bool IsFull(Player player)
    {
        return GetArmiger(player) >= MaxArmiger;
    }

    public static ArmigerData GetDataForUI(Player player)
    {
        return GetData(player);
    }

    public static void Reset(Player player)
    {
        SetArmiger(player, 0);
    }
}