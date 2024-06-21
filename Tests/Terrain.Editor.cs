#if TOOLS
namespace F00F.ShaderNoise.Tests
{
    public partial class Terrain
    {
        public override void _Notification(int what)
        {
            if (Editor.OnPreSave(what))
            {
                if (this.IsSceneRoot())
                {
                    Editor.DoPreSaveReset(Config, x => Config = x);
                    Editor.DoPreSaveReset(Mesh.Mesh, x => Mesh.Mesh = x);
                    Editor.DoPreSaveReset(Shape.Shape, x => Shape.Shape = x);
                    Editor.DoPreSaveReset(Mesh.ExtraCullMargin, x => Mesh.ExtraCullMargin = x);
                    Editor.DoPreSaveReset(Mesh.MaterialOverride, x => Mesh.MaterialOverride = x);
                }

                return;
            }

            if (Editor.OnPostSave(what))
                Editor.DoPostSaveRestore();
        }
    }
}
#endif
