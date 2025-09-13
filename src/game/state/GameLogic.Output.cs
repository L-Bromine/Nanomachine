namespace Nanomachine;


public partial class GameLogic {
    public static class Output {
        public readonly record struct Exit(GameExitReason Reason);
        public readonly record struct IntoPaused;
        public readonly record struct OutofPaused;
        public readonly record struct IntoNewGameSetting;
        public readonly record struct ExitNewGameSetting;
    }
}
