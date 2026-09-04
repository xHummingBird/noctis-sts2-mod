using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Noctis.NoctisCode.Extensions;

public static class NoctisAssets
{
    private static PackedScene? _noctisScene;
    private static PackedScene? _vfxScene;

    private const string NoctisScenePath = "res://Noctis/scenes/noctis.tscn";
    private const string VfxPath = "res://Noctis/scenes/vfx.tscn";
    

    public static PackedScene? NoctisScene
    {
        get
        {
            _noctisScene = LoadOrReload(_noctisScene, NoctisScenePath, "Noctis scene");
            return _noctisScene;
        }
    }

    public static PackedScene? IceScene
    {
        get
        {
            _vfxScene = LoadOrReload(_vfxScene, VfxPath, "Ice VFX");
            return _vfxScene;
        }
    }

    private static PackedScene? LoadOrReload(PackedScene? cachedScene, string path, string label)
    {
        if (cachedScene != null && GodotObject.IsInstanceValid(cachedScene))
            return cachedScene;

        GD.Print($"NoctisAssets: Loading {label} from {path}");

        var scene = GD.Load<PackedScene>(path);

        if (scene == null)
        {
            GD.PrintErr($"NoctisAssets: FAILED to load {label}: {path}");
            return null;
        }

        GD.Print($"NoctisAssets: Loaded {label}");
        return scene;
    }

    public static void EnsurePreloaded()
    {
        _ = NoctisScene;
        _ = IceScene;
        GD.Print("NoctisAssets: EnsurePreloaded finished");
    }
}

[HarmonyPatch(typeof(Hook), nameof(Hook.AfterActEntered))]
public static class NoctisAfterActEnteredPreloadPatch
{
    [HarmonyPrefix]
    public static void Prefix(IRunState runState)
    {
        var player = runState?.Players?.FirstOrDefault();

        if (player?.Character is not Character.Noctis)
            return;

        GD.Print("AfterActEntered: Noctis detected → preloading");

        NoctisAssets.EnsurePreloaded();
    }
}

[HarmonyPatch(typeof(Hook), nameof(Hook.AfterRoomEntered))]
public static class NoctisAfterRoomEnteredPreloadPatch
{
    [HarmonyPrefix]
    public static void Prefix(IRunState runState, AbstractRoom room)
    {
        var player = runState?.Players?.FirstOrDefault();

        if (player?.Character is not Character.Noctis)
            return;

        GD.Print($"AfterRoomEntered: Noctis detected → preloading. Room = {room.GetType().Name}");

        NoctisAssets.EnsurePreloaded();
    }
}
