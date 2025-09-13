namespace Nanomachine;

using Chickensoft.Introspection;

public partial class GameLogic {
    public partial record State {
        [Meta]
        public partial record Playing : State,
        IGet<Input.Back> {
            public Playing() {

                OnAttach(() => Get<IGameRepo>().Ended += OnEnded);
                OnDetach(() => Get<IGameRepo>().Ended -= OnEnded);
            }

            public void OnEnded()
                    => Input(new Input.Exit());

            public Transition On(in Input.Back input) => To<Pauseing>();
            public Transition On(in Input.Exit input) {
                var reason = input.Reason;
                return To<Quit>().With(
                    (state) => ((Quit)state).Reason = reason
                );
            }
        }
    }
}