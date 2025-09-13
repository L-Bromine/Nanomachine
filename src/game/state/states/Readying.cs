namespace Nanomachine;

using Chickensoft.Introspection;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Readying : State,
        IGet<Input.Load> {

            public Transition On(in Input.Load input) {
                return To<Loading>();
            }
        }
    }
}