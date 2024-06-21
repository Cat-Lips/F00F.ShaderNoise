#if TOOLS
namespace F00F.ShaderNoise.Tests
{
    public partial class Main
    {
        public override void _Notification(int what)
        {
            if (Editor.OnPreSave(what))
            {
                Editor.DoPreSaveReset(Camera.Position, x => Camera.Position = x);
                Editor.DoPreSaveReset(Camera.Get("_input"), x => Camera.Set("_input", x));
                Editor.DoPreSaveReset(Camera.Get("_config"), x => Camera.Set("_config", x));
                Editor.DoPreSaveReset(Terrain.Get("_config"), x => Terrain.Set("_config", x));
                return;
            }

            if (Editor.OnPostSave(what))
                Editor.DoPostSaveRestore();
        }
    }
}
#endif
