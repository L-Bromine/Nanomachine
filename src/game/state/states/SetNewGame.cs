namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record SetNewGame : State,
        IGet<Input.FinishSetGame>,
        IGet<Input.Back> {
            public SetNewGame() {
                this.OnEnter(() => Output(new Output.IntoNewGameSetting()));
                this.OnExit(() => Output(new Output.ExitNewGameSetting()));
            }
            public Transition On(in Input.FinishSetGame input) => To<Playing>();
            public Transition On(in Input.Back input) {
                Output(new Output.Exit());
                return To<Readying>();
            }
        }
    }
}