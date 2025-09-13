namespace Nanomachine;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;

using Godot;

public interface IPreviewStar : INode3D {
    public Color Color { set; }
    public float Energy { set; }
    public float Size { set; }
}

[Meta(typeof(IAutoNode))]
public partial class PreviewStar : Node3D, IPreviewStar {
    public override void _Notification(int what) => this.Notify(what);

    [Node] public IMeshInstance3D StarMesh { get; set; } = default!;
    public void Initialize() {
        var material = StarMesh.GetSurfaceOverrideMaterial(0).Duplicate(true);
        StarMesh.SetSurfaceOverrideMaterial(0, (Material)material);
    }

    public Color Color {
        set => SetEmission(value);
    }

    public float Energy {
        set => SetEnergy(value);
    }

    public float Size {
        set => SetSize(value);
    }


    private void SetEmission(Color color) {
        var material = StarMesh.GetSurfaceOverrideMaterial(0);
        if (material is StandardMaterial3D standardMaterial) {
            standardMaterial.Emission = color;
        }
    }

    private void SetEnergy(float energy) {
        var material = StarMesh.GetSurfaceOverrideMaterial(0);
        if (material is StandardMaterial3D standardMaterial) {
            standardMaterial.EmissionEnergyMultiplier = energy;
        }
    }

    private void SetSize(float value) {
        StarMesh.Scale = new Vector3(value, value, value);
    }

}
