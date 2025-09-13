namespace Nanomachine;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;


using Godot;

using Nanomachine.Log;


public interface IGame : INode3D {
    public event Game.SaveFileLoadedEventHandler? SaveFileLoaded;

    public bool LoadGame(string? fileName = null);
    public bool SaveGame(string? filename = null);
    public void ClearGame();

}

[Meta(typeof(IAutoNode))]
public partial class Game : Node3D, IGame {

    #region Save
    [Signal]
    // 保存事件接口
    public delegate void SaveFileLoadedEventHandler();
    #endregion Save

    public void Initialize() {
        // 提供依赖服务，触发依赖此服务的节点的初始化
        this.Provide();
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

/// TODO 为状态机添加Output
/// 为Load添加filename参数的处理