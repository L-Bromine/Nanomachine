namespace Nanomachine;

using Chickensoft.Introspection;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Readying : State,
        IGet<Input.NewStart>,
        IGet<Input.Load> {
            public Transition On(in Input.NewStart input) {
                Get<GameRepo>().NewGame();
                return To<SetNewGame>();
            }

            public Transition On(in Input.Load input) {
                Get<GameRepo>().LoadGame(input.FileName);
                return To<Playing>();
            }
        }
    }
}