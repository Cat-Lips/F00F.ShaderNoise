using Godot;

namespace F00F.ShaderNoise.Tests
{
    [Tool]
    public partial class Main : Game3D
    {
        public Stats Stats => GetNode<Stats>("Stats");
        public Camera Camera => GetNode<Camera>("Camera");
        public Terrain Terrain => GetNode<Terrain>("Terrain");
        public Settings Settings => GetNode<Settings>("Settings");

        #region Godot

        public override void _Ready()
        {
            InitSettings(); ShowSettings();
            Settings.DataSet += InitTerrain;
            Terrain.ConfigSet += InitSettings;
            Camera.SelectModeSet += ShowSettings;
            if (Engine.IsEditorHint()) Settings.Visible = true;

            void InitTerrain()
                => Terrain.Config = Settings.Data;

            void InitSettings()
                => Settings.Data = Terrain.Config;

            void ShowSettings()
                => Settings.Visible = Camera.SelectMode;
        }

        public override void _UnhandledKeyInput(InputEvent e)
        {
            if (e is InputEventKey key)
            {
                if (key.Pressed && !key.Echo)
                {
                    switch (key.PhysicalKeycode)
                    {
                        case Key.F12: ToggleTerrain(); break;
                        case Key.End: QuitGame(); break;
                    }
                }
            }

            void ToggleTerrain()
                => Terrain.Mesh.Visible = !Terrain.Mesh.Visible;

            void QuitGame()
            {
                GetTree().Quit();
                GD.Print("BYE");
            }
        }

        #endregion
    }
}
