namespace Nanomachine;

using Chickensoft.Introspection;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Readying : State,
        IGet<Input.Load> {
            public Transition On(in Input.Load input) {
                var filename = input.FileName;
                return To<Loading>().With(
                    (state) => ((Loading)state).FileName = filename
                );
            }
        }
    }
}