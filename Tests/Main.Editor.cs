#if TOOLS
using F00F;
using Godot;

namespace Tests;

public partial class Main
{
    protected sealed override void _OnEditorSave()
    {
        Editor.DoPreSaveReset(Camera, Node3D.PropertyName.Position);
        Editor.DoPreSaveResetField(Terrain, Terrain.PropertyName.Config);
    }
}
#endif
