namespace Nanomachine;


public partial class GameLogic {
    public static class Input {
        public readonly record struct Load(string FileName);
        public readonly record struct FinishLoadGame;
        public readonly record struct FailLoadGame;

        public readonly record struct Exit(bool QuitApp = false);
        public readonly record struct Back;
        public readonly record struct ToMenu;

        // public readonly record struct ToGalaxy;
        // public readonly record struct ToStellarSystem(IAbStar Star);
        // public readonly record struct ToPlanet(IPlanet Planet);
    }
}
