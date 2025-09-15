namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record IntoingGame : State,
        IGet<Input.FadeOutFinished> {
            public IntoingGame() {
                this.OnEnter(() => Output(new Output.FadeToBlack()));
            }
            public Transition On(in Input.FadeOutFinished input) => To<InGame>();
        }
    }
}

