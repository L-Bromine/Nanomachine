namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Pauseing : Playing,
        IGet<Input.Back>,
        IGet<Input.Exit> {
            public Pauseing() {
                this.OnEnter(() => Output(new Output.IntoPaused()));
                this.OnExit(() => Output(new Output.OutofPaused()));
            }
            public new Transition On(in Input.Back input) => To<Playing>();

        }
    }
}