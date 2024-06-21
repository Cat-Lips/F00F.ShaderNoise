#if TOOLS
using Godot;

namespace F00F.ShaderNoise.Tests;

public partial class Main
{
    public override void _Notification(int what)
    {
        if (Editor.OnPreSave(what))
        {
            Editor.DoPreSaveReset(Camera, Camera.PropertyName.Config);
            Editor.DoPreSaveReset(Camera, Node3D.PropertyName.Position);
            Editor.DoPreSaveReset(Terrain, Terrain.PropertyName.Config);
            return;
        }

        if (Editor.OnPostSave(what))
            Editor.DoPostSaveRestore();
    }
}
#endif
