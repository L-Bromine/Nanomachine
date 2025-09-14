namespace Nanomachine;


public partial class GameLogic {
    public static class Output {
        public readonly record struct Exit(bool QuitApp);
        public readonly record struct IntoPaused;
        public readonly record struct OutofPaused;
        public readonly record struct LoadGame(string? FileName);
    }
}
