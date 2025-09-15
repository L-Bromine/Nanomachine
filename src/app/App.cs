namespace Nanomachine;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;

using Godot;

// 应用程序主接口，定义应用级别的功能
// 实现了ICanvasLayer和依赖注入提供者接口

public interface IApp : ICanvasLayer, IProvide<IAppRepo>;

// 使用AutoNode元数据标记，启用自动节点解析
// 使用ClassDiagram标记，启用类图生成功能
[Meta(typeof(IAutoNode))]
public partial class App : CanvasLayer, IApp {
    // 处理Godot引擎通知，用于依赖注入系统
    public override void _Notification(int what) => this.Notify(what);

    #region Provisions

    // 实现IProvide接口，提供应用数据仓库服务
    IAppRepo IProvide<IAppRepo>.Value() => AppRepo;

    #endregion Provisions

    #region State

    // 应用数据仓库，存储和管理应用级别状态数据
    public IAppRepo AppRepo { get; set; } = default!;

    // 应用逻辑状态机，管理应用程序的核心逻辑和状态转换
    public IAppLogic AppLogic { get; set; } = default!;

    // 状态机绑定，用于处理状态机的输出事件
    public AppLogic.IBinding AppBinding { get; set; } = default!;

    #endregion State

    #region Nodes

    // 使用Chickensoft的自动节点注入特性
    // 这些节点会在场景加载时自动赋值

    [Node] public IGame Game { get; set; } = default!; // 主游戏
    [Node] public IMenu Menu { get; set; } = default!; // 主菜单
    [Node] public IColorRect BlankScreen { get; set; } = default!; // 黑屏遮罩
    [Node] public IAnimationPlayer AnimationPlayer { get; set; } = default!; // 动画播放器
    [Node] public ISplash Splash { get; set; } = default!; // 启动画面
    [Node] public INewGameSettingPanel NewGameSettingPanel { get; set; } = default!;// 新游戏窗口

    #endregion Nodes

    // 初始化方法，设置应用的核心系统
    public void Initialize() {

        // 初始化应用数据仓库和状态机
        AppRepo = new AppRepo();
        AppLogic = new AppLogic();

        // 设置状态机的依赖
        AppLogic.Set(AppRepo);
        AppLogic.Set(new AppLogic.Data());

        // 监听菜单信号，将这些信号转换为状态机的输入
        Menu.NewGame += OnNewGame;
        Menu.LoadGame += OnLoadGame;

        // 监听新游戏设置页面信号
        NewGameSettingPanel.LoadNewGame += OnLoadGame;// 加载临时新游戏文件
        NewGameSettingPanel.BackMenu += OnBackMenu;

        // 监听动画完成事件
        AnimationPlayer.AnimationFinished += OnAnimationFinished;

        Game.ExitGame += OnGameExit;

        // 提供依赖服务，触发依赖此服务的节点的初始化
        this.Provide();

    }

    // 节点准备就绪后调用的方法（类似Godot的_Ready）
    public void OnReady() {
        // 绑定状态机输出处理
        AppBinding = AppLogic.Bind();

        AppBinding
        // 处理显示启动画面输出
            .Handle((in AppLogic.Output.ShowSplashScreen _) => {
                HideMenus(); // 隐藏所有菜单
                BlankScreen.Hide(); // 隐藏黑屏遮罩
                Splash.Show(); // 显示启动画面
            })
        // 处理隐藏启动画面输出
            .Handle((in AppLogic.Output.HideSplashScreen _) => {
                BlankScreen.Show(); // 显示黑屏遮罩
                FadeToBlack(); // 播放淡出到黑屏动画
            })
        // 结束当前游戏，析构
            .Handle((in AppLogic.Output.RemoveExistingGame _) => {
                Game.ClearGame();
                HideMenus();
            })
        // 处理显示主菜单输出
            .Handle((in AppLogic.Output.ShowMainMenu _) => {
                // 在显示黑屏时加载所有内容，然后淡入
                HideMenus(); // 隐藏所有菜单
                Menu.Show(); // 显示主菜单

                FadeInFromBlack(); // 播放从黑屏淡入动画
            })
        // 处理淡出到黑屏输出
            .Handle((in AppLogic.Output.FadeToBlack _) => FadeToBlack())
        // 处理显示游戏输出
            .Handle((in AppLogic.Output.ShowGame _) => {
                HideMenus(); // 隐藏所有菜单
                Game.Show();
                FadeInFromBlack(); // 播放从黑屏淡入动画
            })
        // 处理隐藏游戏输出
            .Handle((in AppLogic.Output.HideGame _) => FadeToBlack())
        // 通知Game节点开始加载存档
          .Handle(
            (in AppLogic.Output.StartLoadingSaveFile output) => Game.CallLoadGame(output.FileName))
          .Handle(
            (in AppLogic.Output.StartSettingNewGame _) => {
                HideMenus(); // 隐藏所有菜单
                NewGameSettingPanel.InitPreview();
                FadeInFromBlack();
            }
          );

        // 启动状态机，触发初始状态的OnEnter回调

        AppLogic.Start();
    }

    // 新游戏按钮事件处理
    public void OnNewGame() => AppLogic.Input(new AppLogic.Input.NewGame());

    // 加载游戏按钮事件处理
    public void OnLoadGame(string? filename) => AppLogic.Input(new AppLogic.Input.LoadGame(filename));

    // 返回主页面按钮事件处理
    public void OnBackMenu() => AppLogic.Input(new AppLogic.Input.EndGame());

    // 动画播放完成事件处理
    public void OnAnimationFinished(StringName animation) {
        // 只有两种动画：淡入和淡出
        // 不关心当前状态，只是通知当前状态动画已完成

        if (animation == "fade_in") {
            // 淡入动画完成
            AppLogic.Input(new AppLogic.Input.FadeInFinished());
            BlankScreen.Hide(); // 隐藏黑屏遮罩
            return;
        }

        // 淡出动画完成
        AppLogic.Input(new AppLogic.Input.FadeOutFinished());
    }

    // 从黑屏淡入动画
    public void FadeInFromBlack() {
        BlankScreen.Show(); // 显示黑屏遮罩
        AnimationPlayer.Play("fade_in"); // 播放淡入动画
    }

    // 淡出到黑屏动画
    public void FadeToBlack() {
        BlankScreen.Show(); // 显示黑屏遮罩
        AnimationPlayer.Play("fade_out"); // 播放淡出动画
    }

    // 隐藏所有菜单
    public void HideMenus() {
        Splash.Hide(); // 隐藏启动画面
        Menu.Hide(); // 隐藏主菜单
        Game.Hide();
        NewGameSettingPanel.ClearPreview(); // 隐藏新游戏设置界面
    }

    public void OnGameExit() => AppRepo.OnExitGame();

    // 节点退出场景树时清理资源
    public void OnExitTree() {
        // 清理资源
        AppLogic.Stop(); // 停止状态机
        AppBinding.Dispose(); // 释放状态机绑定
        AppRepo.Dispose(); // 释放数据仓库

        // 取消事件订阅
        Menu.NewGame -= OnNewGame;
        Menu.LoadGame -= OnLoadGame;
        NewGameSettingPanel.LoadNewGame -= OnLoadGame;
        NewGameSettingPanel.BackMenu -= OnBackMenu;
        AnimationPlayer.AnimationFinished -= OnAnimationFinished;
    }

}
