namespace Nanomachine;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Playing : State,
        IGet<Input.Back>,
        IGet<Input.Exit>,
        IGet<Input.Load> {
            public Playing() {
                this.OnEnter(() => Output(new Output.StartGame()));
                OnAttach(() => Get<IGameRepo>().Ended += OnEnded);
                OnDetach(() => Get<IGameRepo>().Ended -= OnEnded);
            }

            public void OnEnded() => Input(new Input.Exit());

            public Transition On(in Input.Back input) => To<Pauseing>();
            public Transition On(in Input.Exit input) {
                var quitapp = input.QuitApp;
                return To<Quit>().With(
                    (state) => ((Quit)state).QuitApp = quitapp
                );
            }
            public Transition On(in Input.Load input) {
                var filename = input.FileName;
                return To<Loading>().With(
                    (state) => ((Loading)state).FileName = filename
                );
            }
        }
    }
}