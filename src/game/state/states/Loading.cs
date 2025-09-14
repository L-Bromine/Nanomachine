namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Loading : State,
        IGet<Input.FinishLoadGame> {
            public string? FileName { get; set; }
            public Loading() {
                this.OnEnter(() => Output(new Output.LoadGame(FileName)));
                OnAttach(() => {
                    Get<IGameRepo>().LoadFileFinished += OnLoadFileFinished;
                    Get<IGameRepo>().LoadFileFailed += OnLoadFileFailed;
                });
                OnDetach(() => {
                    Get<IGameRepo>().LoadFileFinished -= OnLoadFileFinished;
                    Get<IGameRepo>().LoadFileFailed += OnLoadFileFailed;
                });
            }
            private void OnLoadFileFinished() => Input(new Input.FinishLoadGame());
            private void OnLoadFileFailed() => Input(new Input.Exit());

            public Transition On(in Input.FinishLoadGame input) => To<Playing>();
        }
    }
}