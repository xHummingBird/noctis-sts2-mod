using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using Noctis.NoctisCode.Extensions;
using Noctis.NoctisCode.Mechanics.Armiger;

namespace Noctis.NoctisCode.Mechanic.Armiger;

public partial class ArmigerDisplayOverlay : Control
{
    public static ArmigerDisplayOverlay? Instance { get; private set; }

    private Control? _armigerDisplay;
    private RichTextLabel? _label;
    private Player? _player;
    private IHoverTip? _hoverTip;

    private int _lastValue = -1;
    private Tween? _popTween;
    private bool _exiting;

    private const int ArmigerMax = 100;

    private static readonly Color ArmigerGainBlue =
        new Color(0.35f, 0.65f, 1f);

    private static readonly Color ArmigerMaxGold =
        new Color(1f, 0.85f, 0.2f);

    public override void _Ready()
    {
        Instance = this;
        Name = "ArmigerDisplayOverlay";

        MouseFilter = MouseFilterEnum.Pass;

        CallDeferred(nameof(Setup));
    }

    private async void Setup()
    {
        if (!IsInsideTree())
            return;

        for (int i = 0; i < 60; i++)
        {
            if (_exiting || !IsInsideTree())
                return;

            var state = CombatManager.Instance?.DebugOnlyGetState();

            var player = state?.Players
                .FirstOrDefault(p => LocalContext.IsMe(p));

            if (player != null)
            {
                if (player.Character is not Character.Noctis)
                {
                    QueueFree();
                    return;
                }

                _player = player;
                break;
            }

            var tree = GetTree();

            if (tree == null)
                return;

            await ToSignal(
                tree,
                SceneTree.SignalName.ProcessFrame
            );
        }

        if (_player == null)
        {
            QueueFree();
            return;
        }

        if (_exiting || !IsInsideTree())
            return;

        var scene =
            GD.Load<PackedScene>(
                "res://Noctis/scenes/armiger_display.tscn"
            );

        if (scene == null)
        {
            GD.PushError(
                "[Noctis Armiger] Failed to load armiger_display.tscn"
            );

            QueueFree();
            return;
        }

        _armigerDisplay = scene.Instantiate<Control>();

        AddChild(_armigerDisplay);

        _armigerDisplay.MouseFilter =
            MouseFilterEnum.Ignore;

        _armigerDisplay.SetAnchorsPreset(
            LayoutPreset.BottomLeft
        );

        _armigerDisplay.Position =
            new Vector2(-40, -50);

        _armigerDisplay.Visible = true;

        _label =
            _armigerDisplay.GetNodeOrNull<RichTextLabel>(
                "%ArmigerLabel"
            );

        if (_label == null)
        {
            GD.PushError(
                "[Noctis Armiger] Could not find %ArmigerLabel"
            );

            QueueFree();
            return;
        }

        _label.TreeExiting += () =>
        {
            _popTween?.Kill();
            _popTween = null;
            _label = null;
        };

        var font =
            GD.Load<Font>(
                "res://themes/kreon_bold_shared.tres"
            );

        if (font != null)
        {
            _label.AddThemeFontOverride(
                "font",
                font
            );

            _label.AddThemeFontOverride(
                "normal_font",
                font
            );
        }

        _label.AddThemeColorOverride(
            "default_color",
            Colors.White
        );

        _label.Position += new Vector2(0, 29);

        _label.AddThemeColorOverride(
            "font_outline_color",
            new Color(0.2f, 0.2f, 0.2f)
        );

        _label.AddThemeConstantOverride(
            "outline_size",
            10
        );

        _label.AddThemeFontSizeOverride(
            "normal_font_size",
            30
        );
        
        _label.PivotOffset = _label.Size * 0.5f;

        _hoverTip = NoctisStaticHoverTip.Armiger;

        _label.MouseFilter = MouseFilterEnum.Pass;

        _label.Connect(
            SignalName.MouseEntered,
            Callable.From(OnHovered)
        );

        _label.Connect(
            SignalName.MouseExited,
            Callable.From(OnUnhovered)
        );

        MouseFilter = MouseFilterEnum.Pass;

        Connect(
            SignalName.MouseEntered,
            Callable.From(OnHovered)
        );

        Connect(
            SignalName.MouseExited,
            Callable.From(OnUnhovered)
        );

        var data =
            ArmigerManager.GetDataForUI(_player);

        data.OnArmigerChanged += OnArmigerChanged;

        UpdateDisplay(
            ArmigerManager.GetArmiger(_player)
        );
    }

    private void PlayGainPop(bool isMaxed)
    {
        if (_exiting)
            return;

        var label = _label;

        if (label == null)
            return;

        if (!GodotObject.IsInstanceValid(label))
            return;

        if (label.IsQueuedForDeletion())
            return;

        if (_popTween != null &&
            GodotObject.IsInstanceValid(_popTween))
        {
            _popTween.Kill();
        }

        label.Scale = Vector2.One;
        label.Modulate = ArmigerGainBlue;

        _popTween = label.CreateTween();

        _popTween
            .TweenProperty(
                label,
                "scale",
                new Vector2(1.25f, 1.25f),
                0.10f
            )
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);

        _popTween
            .TweenProperty(
                label,
                "scale",
                Vector2.One,
                0.40f
            )
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);

        _popTween.Parallel()
            .TweenProperty(
                label,
                "modulate",
                isMaxed
                    ? ArmigerMaxGold
                    : Colors.White,
                0.40f
            )
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
    }

    private void OnHovered()
    {
        if (_exiting)
            return;

        if (_hoverTip == null)
            return;

        NHoverTipSet.Clear();

        var tip =
            NHoverTipSet.CreateAndShow(
                this,
                _hoverTip
            );

        tip.GlobalPosition =
            GlobalPosition +
            new Vector2(-75f, -450f);

        tip.MouseFilter =
            MouseFilterEnum.Ignore;
    }

    private void OnUnhovered()
    {
        NHoverTipSet.Remove(this);
    }

    private void OnArmigerChanged(int value)
    {
        UpdateDisplay(value);
    }

    private void UpdateDisplay(int value)
    {
        if (_exiting)
            return;

        var label = _label;

        if (label == null)
            return;

        if (!GodotObject.IsInstanceValid(label))
            return;

        if (label.IsQueuedForDeletion())
            return;

        try
        {
            label.Text =
                $"[center]{value}[/center]";
        }
        catch (ObjectDisposedException)
        {
            return;
        }

        bool isMaxed =
            value >= ArmigerMax;

        if (_lastValue >= 0 &&
            value > _lastValue)
        {
            PlayGainPop(isMaxed);
        }
        else
        {
            label.Scale = Vector2.One;

            label.Modulate =
                isMaxed
                    ? ArmigerMaxGold
                    : Colors.White;
        }

        _lastValue = value;
    }

    public override void _ExitTree()
    {
        _exiting = true;

        if (_popTween != null &&
            GodotObject.IsInstanceValid(_popTween))
        {
            _popTween.Kill();
        }

        _popTween = null;

        if (_player != null)
        {
            var data =
                ArmigerManager.GetDataForUI(_player);

            data.OnArmigerChanged -=
                OnArmigerChanged;
        }

        NHoverTipSet.Remove(this);

        _label = null;
        _armigerDisplay = null;
        _player = null;
        _hoverTip = null;

        if (Instance == this)
            Instance = null;
    }
}

[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter._Ready))]
public static class NoctisArmigerDisplayOverlayPatch
{
    public static void Postfix(NEnergyCounter __instance)
    {
        if (__instance == null)
            return;

        if (!GodotObject.IsInstanceValid(__instance))
            return;

        if (__instance.IsQueuedForDeletion())
            return;

        if (__instance.GetNodeOrNull<ArmigerDisplayOverlay>(
                "ArmigerDisplayOverlay") != null)
        {
            return;
        }

        var overlay =
            new ArmigerDisplayOverlay
            {
                Name = "ArmigerDisplayOverlay"
            };

        __instance.AddChild(overlay);
    }
}