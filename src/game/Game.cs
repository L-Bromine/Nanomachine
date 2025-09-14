namespace Nanomachine;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;


using Godot;

using Nanomachine.Log;

public interface IGame : INode3D, IProvide<IGameRepo> {
    public event Game.LoadGameFinishedEventHandler LoadGameFinished;
    public event Game.ExitGameEventHandler ExitGame;

    public bool LoadGame(string? fileName = null);
    public bool SaveGame(string? filename = null);
    public void ClearGame();

}

[Meta(typeof(IAutoNode))]
public partial class Game : Node3D, IGame {
    public override void _Notification(int what) => this.Notify(what);

    #region Provisions

    // 实现IProvide接口，提供应用数据仓库服务
    IGameRepo IProvide<IGameRepo>.Value() => GameRepo;

    #endregion Provisions

    #region State

    // 应用数据仓库，存储和管理应用级别状态数据
    public IGameRepo GameRepo { get; set; } = default!;

    // 应用逻辑状态机，管理应用程序的核心逻辑和状态转换
    public IGameLogic GameLogic { get; set; } = default!;

    // 状态机绑定，用于处理状态机的输出事件
    public GameLogic.IBinding GameBinding { get; set; } = default!;

    #endregion State

    [Signal] public delegate void ExitGameEventHandler();
    [Signal] public delegate void LoadGameFinishedEventHandler();

    public void Initialize() {
        GameRepo = new GameRepo();
        GameLogic = new GameLogic();

        // 设置状态机的依赖
        GameLogic.Set(GameLogic);
        GameLogic.Set(new GameLogic.Data());

        // 提供依赖服务，触发依赖此服务的节点的初始化
        this.Provide();
    }

    public void OnReady() {
        // 绑定状态机输出处理
        GameBinding = GameLogic.Bind();

        GameBinding
            .Handle((in GameLogic.Output.Exit output) => {
                if (output.QuitApp) {
                    GetTree().Quit(); // 直接强制退出
                }
                else {
                    EmitSignal(SignalName.ExitGame);
                    GameLogic.Input(new GameLogic.Input.ToMenu());
                }
            })
            .Handle((in GameLogic.Output.LoadGame output) => {
                // TODO异步启动加载动画
                // 加载游戏
                var flag = LoadGame(output.FileName);
                if (flag) {
                    GameRepo.OnLoadFileFinished();
                }
                else {
                    GameRepo.OnLoadFileFailed();
                }
            })
            .Handle((in GameLogic.Output.IntoPaused _) => { })
            .Handle((in GameLogic.Output.OutofPaused _) => { })
            ;
    }

    public bool LoadGame(string? fileName) {
        if (fileName == null) {
            Logger.d.Log($"空存档被加载");
            return false;
        }
        Logger.i.Log($"正在加载游戏存档：{fileName}");
        // TODO 加载游戏存档
        return false;
    }

    public bool SaveGame(string? filename = null) => throw new System.NotImplementedException();
    public void ClearGame() {
        Logger.i.Log($"正在析构游戏……");
        // TODO 析构游戏
    }
}