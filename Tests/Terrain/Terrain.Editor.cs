#if TOOLS
using F00F;
using Godot;

namespace Tests;

public partial class Terrain
{
    public sealed override void _Notification(int what)
    {
        if (Editor.OnPreSave(what))
        {
            if (this.IsEditedSceneRoot())
                Editor.DoPreSaveResetField(this, PropertyName.Config);

            Editor.DoPreSaveReset(Mesh, MeshInstance3D.PropertyName.Mesh);
            Editor.DoPreSaveReset(Shape, CollisionShape3D.PropertyName.Shape);
            Editor.DoPreSaveReset(Mesh, GeometryInstance3D.PropertyName.ExtraCullMargin);
            Editor.DoPreSaveReset(Mesh, GeometryInstance3D.PropertyName.MaterialOverride);

            return;
        }

        if (Editor.OnPostSave(what))
            Editor.DoPostSaveRestore();
    }
}
#endif
