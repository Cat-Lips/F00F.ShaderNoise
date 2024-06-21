using System;
using F00F;
using F00F.ShaderNoise;
using Godot;

namespace Tests;

public enum ShapeType
{
    Polygon,
    HeightMap,
}

[Tool, GlobalClass]
public partial class TerrainData : Resource
{
    public event Action NoiseSet;
    public event Action AmplitudeSet;
    public event Action ShapeTypeSet;
    public event Action ShapeSizeSet;
    public event Action TerrainSizeSet;
    public event Action TerrainTintSet;

    #region Export

    [Export] public ShaderNoise2D Noise { get; set => this.Set(ref field, value, NoiseSet); } = new();
    [Export] public float Amplitude { get; set => this.Set(ref field, value, AmplitudeSet); } = 25;
    [Export] public ShapeType ShapeType { get; set => this.Set(ref field, value, ShapeTypeSet); } = ShapeType.HeightMap;
    [Export] public int ShapeSize { get; set => this.Set(ref field, value.ToPo2(field), ShapeSizeSet); } = 32;
    [Export] public int TerrainSize { get; set => this.Set(ref field, value.ToPo2(field), TerrainSizeSet); } = 512;
    [Export(PropertyHint.ColorNoAlpha)] public Color TerrainTint { get; set => this.Set(ref field, value, TerrainTintSet); } = Colors.ForestGreen;

    #endregion

    public float GetHeight(float x, float z)
        => Noise is null ? 0 : Noise.GetNoise(x, z) * Amplitude;
}
