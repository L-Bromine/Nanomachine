namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;


public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record LeavingMenu : State, IGet<Input.FadeOutFinished> {
            public Transition? TO { get; set; }

            public LeavingMenu() {
                this.OnEnter(() => Output(new Output.FadeToBlack()));
            }

            public Transition On(in Input.FadeOutFinished input) =>
              TO ?? To<MainMenu>();
        }
    }
}
