namespace Nanomachine;

using Chickensoft.GodotNodeInterfaces;

using Godot;

public interface IPlanet : INode3D {
}

public partial class Planet : Node3D, IPlanet { }