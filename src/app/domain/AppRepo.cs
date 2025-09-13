namespace Nanomachine;

using System;

/// <summary>
/// 纯应用程序游戏逻辑仓库，在视图特定逻辑块之间共享。
/// </summary>
public interface IAppRepo : IDisposable {


    /// <summary>
    /// 游戏即将开始时触发的事件。
    /// </summary>
    public event Action? GameEntered;

    /// <summary>
    /// 游戏即将结束时触发的事件。
    /// </summary>
    public event Action? GameExited;

    /// <summary>跳过启动画面时触发的事件。</summary>
    public event Action? SplashScreenSkipped;

    /// <summary>进入主菜单时触发的事件。</summary>
    public event Action? MainMenuEntered;

    /// <summary>加载游戏开始时触发的事件。</summary>
    public event Action<string?>? GameLoading;

    /// <summary>加载游戏结束时触发的事件。</summary>
    public event Action<bool>? GameLoaded;

    /// <summary>设置新游戏结束时触发的事件。</summary>
    public event Action<string?>? FinishSettingNewGame;


    /// <summary>通知应用应该显示游戏。</summary>
    public void OnEnterGame();

    /// <summary>通知应用应该退出游戏。</summary>
    public void OnExitGame();

    /// <summary>通知应用开始加载一个存档。</summary>
    /// <param name="filename">需要加载的存档名</param>
    public void OnLoadFile(string? filename);

    /// <summary>通知应用一个新存档的设置已结束。</summary>
    /// <param name="filename">新存档的存档名</param>
    public void OnFinishStartNewGame(string? filename);

    /// <summary>通知应用已进入主菜单。</summary>
    public void OnMainMenuEntered();

    /// <summary>跳过启动画面。</summary>
    public void SkipSplashScreen();
}

/// <summary>
/// 纯应用程序游戏逻辑仓库 - 在视图特定逻辑块之间共享。
/// </summary>
public class AppRepo : IAppRepo {
    // 实现接口定义的事件
    public event Action? SplashScreenSkipped;
    public event Action? MainMenuEntered;
    public event Action? GameEntered;
    public event Action? GameExited;
    public event Action<string?>? GameLoading;
    public event Action<bool>? GameLoaded;
    public event Action<string?>? FinishSettingNewGame;


    private bool _disposedValue;

    // 跳过启动画面 - 触发相应事件
    public void SkipSplashScreen() => SplashScreenSkipped?.Invoke();

    // 进入主菜单 - 触发相应事件
    public void OnMainMenuEntered() => MainMenuEntered?.Invoke();

    // 进入游戏 - 触发相应事件
    public void OnEnterGame() => GameEntered?.Invoke();

    // 退出游戏 - 触发相应事件
    public void OnExitGame() => GameExited?.Invoke();

    // 加载游戏 - 触发相应事件并传递文件名
    public void OnLoadFile(string? filename) => GameLoading?.Invoke(filename);

    // 结束设置新游戏 - 触发相应事件并传递文件名
    public void OnFinishStartNewGame(string? filename) => FinishSettingNewGame?.Invoke(filename);


    #region Internals

    // 实现IDisposable接口，清理事件订阅
    protected void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // 清理托管对象（事件订阅）
                SplashScreenSkipped = null;
                MainMenuEntered = null;
                GameEntered = null;
                GameExited = null;
            }

            _disposedValue = true;
        }
    }

    // 公共Dispose方法
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }


    #endregion Internals
}