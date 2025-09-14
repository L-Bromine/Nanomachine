namespace Nanomachine;

using System;

public interface IGameRepo : IDisposable {
    public event Action? LoadFileFinished;
    public event Action? LoadFileFailed;

    /// <summary>Event invoked when the game ends.</summary>
    public event Action? Ended;

    public void OnEnded();
    public void OnLoadFileFailed();
    public void OnLoadFileFinished();

}

/// <summary>
///   Game repository — stores pure game logic that's not directly related to the
///   game node's overall view.
/// </summary>
public class GameRepo : IGameRepo {
    public event Action? Ended;
    public event Action? LoadFileFinished;
    public event Action? LoadFileFailed;

    public void OnEnded() => Ended?.Invoke();
    public void OnLoadFileFailed() => LoadFileFailed?.Invoke();
    public void OnLoadFileFinished() => LoadFileFinished?.Invoke();

    #region Internals
    public void Dispose() => GC.SuppressFinalize(this);

    #endregion Internals
}
