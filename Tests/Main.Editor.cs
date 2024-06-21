#if TOOLS
using F00F;
using Godot;
using Camera3D = F00F.Camera3D;

namespace Tests;

public partial class Main
{
    protected override void OnEditorSave()
    {
        Editor.DoPreSaveReset(Camera, Node3D.PropertyName.Position);
        Editor.DoPreSaveResetField(Camera, Camera3D.PropertyName.Config);
        Editor.DoPreSaveResetField(Terrain, Terrain.PropertyName.Config);
    }
}
#endif
