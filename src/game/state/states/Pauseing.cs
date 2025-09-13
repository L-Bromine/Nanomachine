namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Pauseing : State,
        IGet<Input.Back>,
        IGet<Input.Exit> {
            public Pauseing() {
                this.OnEnter(() => Output(new Output.IntoPaused()));
                this.OnExit(() => Output(new Output.OutofPaused()));
            }
            public Transition On(in Input.Back input) => To<Playing>();
            public Transition On(in Input.Exit input) {
                var reason = input.Reason;
                return To<Quit>().With(
                    (state) => ((Quit)state).Reason = reason
                );
            }
        }
    }
}