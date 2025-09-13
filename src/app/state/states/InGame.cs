namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class AppLogic {
    public partial record State {
        [Meta]
        public partial record InGame : State,
        IGet<Input.EndGame>,
        IGet<Input.LoadGame> {
            public InGame() {
                this.OnEnter(() => {
                    Get<IAppRepo>().OnEnterGame();
                    Output(new Output.ShowGame());
                });
                this.OnExit(() => Output(new Output.HideGame()));

                OnAttach(() => {
                    Get<IAppRepo>().GameExited += OnGameExited;
                    Get<IAppRepo>().GameLoading += OnGameLoading;
                });
                OnDetach(() => {
                    Get<IAppRepo>().GameExited -= OnGameExited;
                    Get<IAppRepo>().GameLoading -= OnGameLoading;
                });
            }

            public void OnGameExited() =>
                Input(new Input.EndGame());
            public void OnGameLoading(string? filename) =>
                Input(new Input.LoadGame(filename));

            public Transition On(in Input.EndGame input) => To<LeavingGame>();
            public Transition On(in Input.LoadGame input) {
                Output(new Output.StartLoadingSaveFile(input.FileName));
                return ToSelf();
            }
        }
    }
}
