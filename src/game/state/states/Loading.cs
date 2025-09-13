namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Loading : State,
        IGet<Input.FinishLoadGame> {
            public Loading() {
            }
            public Transition On(in Input.FinishLoadGame input) => To<Playing>();
        }
    }
}