namespace Nanomachine;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;

using Godot;

using Nanomachine.Log;
using Nanomachine.Render;

using System;

using System.Collections.Generic;

using System.Text.Json.Nodes;

public interface INewGameSettingPanel : IControl {
    public event NewGameSettingPanel.LoadNewGameEventHandler LoadNewGame;
    public event NewGameSettingPanel.BackMenuEventHandler BackMenu;
}

[Meta(typeof(IAutoNode))]
public partial class NewGameSettingPanel : Control, INewGameSettingPanel {
    // 处理Godot引擎通知，用于依赖注入系统
    public override void _Notification(int what) => this.Notify(what);

    // 场景实例化器，用于动态加载和实例化场景
    public IInstantiator Instantiator { get; set; } = default!;

    // 恒星预览场景地址
    public const string PREVIEW_STAR_SCENE_PATH = @"res://src/ui/newGameSettingPanel/PreviewStar/PreviewStar.tscn";

    #region Signals
    [Signal]
    public delegate void LoadNewGameEventHandler(string? newgamename);
    [Signal]
    public delegate void BackMenuEventHandler();
    #endregion Signals

    #region Nodes
    // 使用Chickensoft的自动节点注入特性
    // 这些节点会在场景加载时自动赋值
    [Node] public INode3D GalaxyPreview { get; set; } = default!;
    [Node] public IButton NewGameButton { get; set; } = default!;
    [Node] public IButton BackMenuButton { get; set; } = default!;
    [Node] public IHSlider StarCountSlider { get; set; } = default!;
    [Node] public ILabel StarCountLabel { get; set; } = default!;
    [Node] public IButton RandSeedButtom { get; set; } = default!;
    [Node] public ILabel SeedLabel { get; set; } = default!;

    #endregion Nodes

    #region Colors
    private ColorGradient _gradient = new(new Dictionary<float, Color>{
        {0f, new Color("#85140044")},
        {0.22f, new Color("#E3050166")},
        {0.5f, new Color("#E9D30088")},
        {0.6f, new Color("#FFECEE88")},
        {0.75f, new Color("#64ECF4AA")},
        {0.9f, new Color("#0075F3FF")},
        {0.98f, new Color("#61009E11")},
        {1f, new Color("#30005011")}
    });
    #endregion Colors

    private int _number = 100;
    private float _maxX = 50;
    private float _maxY = 50;
    private float _zRange = 2;
    private int _seed;
    public void Initialize() {
        // 初始化场景实例化器，使用当前场景树
        Instantiator = new Instantiator(GetTree());

        // 预加载PreviewStar
        Instantiator.Load(PREVIEW_STAR_SCENE_PATH);

        // 提供依赖服务，触发依赖此服务的节点的初始化
        this.Provide();
    }
    public void OnReady() {
        NewGameButton.Pressed += OnNewGamePressed;
        BackMenuButton.Pressed += OnLoadGamePressed;
        StarCountSlider.ValueChanged += OnCountSliderChanged;
        RandSeedButtom.Pressed += OnRandSeed;

        OnRandSeed(); // 随机初始化星图
    }

    public void OnExitTree() {
        NewGameButton.Pressed -= OnNewGamePressed;
        BackMenuButton.Pressed -= OnLoadGamePressed;
        StarCountSlider.ValueChanged -= OnCountSliderChanged;
        RandSeedButtom.Pressed -= OnRandSeed;
    }

    public void OnLoadGamePressed() => EmitSignal(SignalName.BackMenu);
    public void OnNewGamePressed() {

        Logger.d.Log("正在暂存游戏：newgame.json");
        EmitSignal(SignalName.LoadNewGame, "save/newgame.json");
    }
    private void OnCountSliderChanged(double value) {
        _number = (int)value;
        StarCountLabel.Text = $"恒星数量：{_number}";
        ChangePreView();
    }

    private void OnRandSeed() {
        var random = new Random((int)Time.GetTicksMsec());
        _seed = random.Next();
        SeedLabel.Text = $"{_seed}：随机种子";
        ChangePreView();
    }

    private void ChangePreView() {
        var galaxy = GalaxyGenerator.RandomGalaxy(_seed, new JsonObject {
            ["maxX"] = _maxX,
            ["maxY"] = _maxY,
            ["zRange"] = _zRange,
            ["n"] = _number
        });

        // 清除之前的所有节点
        foreach (var child in GalaxyPreview.GetChildren()) {
            child.QueueFree();
        }

        if (galaxy.TryGetPropertyValue("stars", out var starsNode) &&
                    starsNode is JsonArray starsNodeArray) {
            foreach (var node in starsNodeArray) {
                if (node == null) { continue; }
                var star = node.AsObject();

                var x = star["x"]?.GetValue<float>();
                var y = star["y"]?.GetValue<float>();
                var z = star["z"]?.GetValue<float>();
                var type = (star["type"]?.GetValue<float>()) ?? 0;

                var starObject = Instantiator.Instantiate<PreviewStar>();
                if (starObject == null) {
                    Logger.e.Log("得到的prestar场景为null");
                    continue;
                }
                GalaxyPreview.AddChild(starObject);

                starObject.Position = new Vector3(x ?? 0, z ?? 0, y ?? 0); // 将xyz坐标转化为xzy坐标

                var color = _gradient.GetColor(type);
                var size = color.A;
                color.A = 1;
                starObject.Color = color;
                starObject.Energy = (10.0f * type) + 2f;
                starObject.Size = size;

            }
        }
        else {
            Logger.e.Log("无法获得stars数组");
        }
    }
}
