namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;


public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record LoadingSaveFile : State,
        IGet<Input.SaveFileLoaded> {
            public string? FileName { get; set; }
            public LoadingSaveFile() {
                this.OnEnter(() => Output(new Output.StartLoadingSaveFile(FileName)));
            }

            public Transition On(in Input.SaveFileLoaded input) {
                if (input.Flag) {
                    return To<InGame>();
                }
                else {
                    return To<MainMenu>();
                }
            }

        }
    }
}
