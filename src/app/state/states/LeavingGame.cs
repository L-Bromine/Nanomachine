namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;


public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record LeavingGame : State, IGet<Input.FadeOutFinished> {
            public LeavingGame() {
                this.OnEnter(() => Output(new Output.FadeToBlack()));
            }

            public Transition On(in Input.FadeOutFinished input) {
                // 前往游戏结束后的页面
                Output(new Output.RemoveExistingGame());
                return To<MainMenu>();
            }
        }
    }
}

