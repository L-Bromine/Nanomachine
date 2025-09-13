namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record SettingNewGame : State,
        IGet<Input.LoadGame>,
        IGet<Input.EndGame> {
            public SettingNewGame() {
                this.OnEnter(() => Output(new Output.StartSettingNewGame()));

                OnAttach(() => Get<IAppRepo>().FinishSettingNewGame += OnFinishStartNewGame);
                OnDetach(() => Get<IAppRepo>().FinishSettingNewGame -= OnFinishStartNewGame);
            }

            public void OnFinishStartNewGame(string? filename) => Input(new Input.LoadGame(filename));

            public Transition On(in Input.LoadGame input) {
                var filename = input.FileName;
                return To<LoadingSaveFile>().With(
                    (state) => ((LoadingSaveFile)state).FileName = filename);
            }
            public Transition On(in Input.EndGame input) => To<LeavingGame>();
        }
    }
}

