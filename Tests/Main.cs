using F00F;
using Godot;

namespace Tests;

[Tool]
public partial class Main : Game3D
{
    public Stats Stats => GetNode<Stats>("Stats");
    public Terrain Terrain => GetNode<Terrain>("Terrain");
    public Settings Settings => GetNode<Settings>("Settings");

    protected override IGameConfig GameConfig => field ??= new TestConfig();

    #region Godot

    protected override void OnReady()
    {
        InitSettings(); ShowSettings();
        Settings.DataSet += InitTerrain;
        Terrain.ConfigSet += InitSettings;
        Camera.SelectModeSet += ShowSettings;
        if (Editor.IsEditor) Settings.Visible = true;

        void InitTerrain()
            => Terrain.Config = Settings.Data;

        void InitSettings()
            => Settings.Data = Terrain.Config;

        void ShowSettings()
            => Settings.Visible = Camera.SelectMode;
    }

    protected sealed override bool OnUnhandledKeyInput(InputEventKey key)
    {
        return this.Handle(key.IsActionPressed(MyInput.ToggleTerrain), ToggleTerrain);

        void ToggleTerrain()
            => Terrain.Mesh.Visible = !Terrain.Mesh.Visible;
    }

    #endregion
}
