namespace Nanomachine;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;

using Godot;

public interface IMenu : IControl {
    public event Menu.NewGameEventHandler NewGame;
    public event Menu.LoadGameEventHandler LoadGame;
}

[Meta(typeof(IAutoNode))]
public partial class Menu : Control, IMenu {
    public override void _Notification(int what) => this.Notify(what);

    #region Nodes
    [Node]
    public IButton NewGameButton { get; set; } = default!;
    [Node]
    public IButton LoadGameButton { get; set; } = default!;
    #endregion Nodes

    #region Signals
    [Signal]
    public delegate void NewGameEventHandler();
    [Signal]
    public delegate void LoadGameEventHandler(string? filename);
    #endregion Signals

    public void OnReady() {
        NewGameButton.Pressed += OnNewGamePressed;
        LoadGameButton.Pressed += OnLoadGamePressed;
    }

    public void OnExitTree() {
        NewGameButton.Pressed -= OnNewGamePressed;
        LoadGameButton.Pressed -= OnLoadGamePressed;
    }

    public void OnNewGamePressed() => EmitSignal(SignalName.NewGame);

    /// 点击加载按钮事件，暂时返回null
    public void OnLoadGamePressed() => EmitSignal(SignalName.LoadGame, null);
}
