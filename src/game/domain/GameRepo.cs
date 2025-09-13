namespace Nanomachine;

using Chickensoft.Collections;
using Chickensoft.LogicBlocks;

using Godot;

using System;

public interface IGameRepo : IDisposable {
    /// <summary>Event invoked when the game ends.</summary>
    public event Action? Ended;

    public void NewGame();
    public void LoadGame(string fileName);

    public void Pause();
    public void UnPause();

}

/// <summary>
///   Game repository — stores pure game logic that's not directly related to the
///   game node's overall view.
/// </summary>
public class GameRepo : IGameRepo {
    public event Action? Ended;

    private GameExitReason? _exitReason;


    public void LoadGame(string fileName) => throw new NotImplementedException();
    public void NewGame() {

    }


    #region Internals
    public void Dispose() => GC.SuppressFinalize(this);
    public void Pause() => throw new NotImplementedException();
    public void UnPause() => throw new NotImplementedException();

    #endregion Internals
}
