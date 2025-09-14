namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Quit : State, IGet<Input.ToMenu> {
            public bool QuitApp { get; set; }

            public Quit() {
                this.OnEnter(() => Output(new Output.Exit(QuitApp)));
            }

            public Transition On(in Input.ToMenu input) => To<Readying>();

        }
    }
}
