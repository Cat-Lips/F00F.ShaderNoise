using System;
using Godot;
using static F00F.ShaderNoise.PerlinNoise.Const;

namespace F00F.ShaderNoise.Tests
{
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

        [Export] public PerlinNoise Noise { get; set => this.Set(ref field, value, NoiseSet); } = new();
        [Export] public float Amplitude { get; set => this.Set(ref field, value, AmplitudeSet); } = 25;
        [Export] public ShapeType ShapeType { get; set => this.Set(ref field, value, ShapeTypeSet); } = ShapeType.HeightMap;
        [Export] public int ShapeSize { get; set => this.Set(ref field, value.ToPo2(field), ShapeSizeSet); } = 32;
        [Export] public int TerrainSize { get; set => this.Set(ref field, value.ToPo2(field), TerrainSizeSet); } = 512;
        [Export(PropertyHint.ColorNoAlpha)] public Color TerrainTint { get; set => this.Set(ref field, value, TerrainTintSet); } = Colors.ForestGreen;

        #endregion

        public float GetHeight(float x, float y, float z)
        {
            return Amplitude * (GetNoise() ?? 0);

            float? GetNoise() => Noise?.NoiseType switch
            {
                null => null,
                NoiseType.Noise2D => Noise.GetNoise(x, z),
                NoiseType.Noise3D => Noise.GetNoise(x, y, z),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
