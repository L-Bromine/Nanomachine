namespace Nanomachine;


public partial class GameLogic {
    public static class Input {
        public readonly record struct NewStart;
        public readonly record struct Load(string FileName);
        public readonly record struct FinishSetGame;

        public readonly record struct Exit(GameExitReason Reason);
        public readonly record struct Back;

        // public readonly record struct ToGalaxy;
        // public readonly record struct ToStellarSystem(IAbStar Star);
        // public readonly record struct ToPlanet(IPlanet Planet);
    }
}
