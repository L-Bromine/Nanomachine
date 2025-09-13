namespace Nanomachine;

using Godot;

using Chickensoft.GameTools.Displays;

#if RUN_TESTS
using System.Reflection;

using Chickensoft.GoDotTest;

using Nanomachine.Log;

using Chickensoft.Log.Godot;
#endif

// 此入口点文件负责决定是否应运行测试。



public partial class Main : Node2D {
    public Vector2I DesignResolution => Display.UHD4k;
#if RUN_TESTS
    public TestEnvironment Environment = default!;
#endif

    public override void _Ready() {
        // 初始化Log系统
        Logger.Initialize(new GDWriter());

        // 校正任何错误的缩放并猜测合理的默认值。

        GetWindow().LookGood(WindowScaleBehavior.UIFixed, DesignResolution);

#if RUN_TESTS
        // 如果这是调试版本，使用 GoDotTest 来检查命令行参数并决定是否应运行测试。
        Environment = TestEnvironment.From(OS.GetCmdlineArgs());
        if (Environment.ShouldRunTests) {
            CallDeferred("RunTests");
            return;
        }
#endif

        // 如果不需要运行测试，我们可以直接切换到游戏场景。
        CallDeferred("RunScene");
    }

#if RUN_TESTS
    private void RunTests()
      => _ = GoTest.RunTests(Assembly.GetExecutingAssembly(), this, Environment);
#endif

    private void RunScene()
      => GetTree().ChangeSceneToFile("res://src/app/App.tscn");
}
