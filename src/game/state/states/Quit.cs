namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Quit : State {
            public GameExitReason Reason { get; set; }
            public string? FileName { get; set; }
            public Quit() {
                this.OnEnter(
                () => {
                    switch (Reason) {
                        case GameExitReason.BackToMenu:
                            Get<IAppRepo>().OnExitGame();
                            break;
                        case GameExitReason.LoadNewGame:
                            Get<IAppRepo>().OnLoadFile(FileName);
                            break;
                        case GameExitReason.QuitToDeskTop:
                            Get<IApp>().GetTree().Quit(); // 直接强制退出
                            break;
                        default:
                            break;
                    }
                });
            }


        }
    }
}
