namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record MainMenu : State,
        IGet<Input.NewGame>, IGet<Input.LoadGame> {
            public MainMenu() {
                this.OnEnter(
                  () => {
                      //   Output(new Output.SetupGameScene());

                      Get<IAppRepo>().OnMainMenuEntered();

                      Output(new Output.ShowMainMenu());
                  }
                );
            }

            public Transition On(in Input.NewGame input) => To<LeavingMenu>()
                .With((state) => ((LeavingMenu)state).TO = To<SettingNewGame>());

            public Transition On(in Input.LoadGame input) {
                Get<Data>().RunningFile = input.FileName;
                return To<LeavingMenu>()
                    .With((state) => ((LeavingMenu)state).TO = To<IntoingGame>());
            }
        }
    }
}
