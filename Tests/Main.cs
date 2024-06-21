using F00F;
using Godot;

namespace Tests;

[Tool]
public partial class Main : Test3D
{
    public Terrain Terrain => GetNode<Terrain>("Terrain");

    protected sealed override void InitSettings()
        => Settings.SetData("Terrain", Terrain.Config, x => Terrain.Config = x);
}
