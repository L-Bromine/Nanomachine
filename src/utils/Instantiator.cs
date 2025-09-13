namespace Nanomachine;

using Godot;

/// <summary>
/// 用于加载和实例化场景的实用工具类
/// </summary>
public interface IInstantiator {
    /// <summary>场景树</summary>
    public SceneTree SceneTree { get; }

    /// <summary>
    /// 加载并实例化指定路径的场景
    /// </summary>
    /// <param name="path">场景文件的路径</param>
    /// <typeparam name="T">场景根节点的类型</typeparam>
    /// <returns>场景的实例</returns>
    public T LoadAndInstantiate<T>(string? path) where T : Node;

    public void Load(string? path);
    public T? Instantiate<T>() where T : Node;
    public void Clear();
}

/// <summary>
/// 用于加载和实例化场景的实用工具类
/// </summary>
public class Instantiator : IInstantiator {
    public SceneTree SceneTree { get; }

    private PackedScene? _scene;

    public Instantiator(SceneTree sceneTree) {
        SceneTree = sceneTree;
    }

    public T LoadAndInstantiate<T>(string? path) where T : Node {
        var scene = GD.Load<PackedScene>(path);
        return scene.Instantiate<T>();
    }

    public void Load(string? path) => _scene = GD.Load<PackedScene>(path);

    public T? Instantiate<T>() where T : Node => _scene?.Instantiate<T>();

    public void Clear() => _scene = null;
}