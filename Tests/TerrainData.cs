using System;
using Godot;

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
        #region Private

        private ShaderNoise2D _noise = new();
        private float _amplitude = 25;
        private ShapeType _shapeType = ShapeType.HeightMap;
        private int _shapeSize = 32;
        private int _terrainSize = 512;
        private Color _terrainTint = Colors.ForestGreen;

        #endregion

        #region Export

        [Export] public ShaderNoise2D Noise { get => _noise; set => this.Set(ref _noise, value, NoiseSet); }
        [Export] public float Amplitude { get => _amplitude; set => this.Set(ref _amplitude, value, AmplitudeSet); }
        [Export] public ShapeType ShapeType { get => _shapeType; set => this.Set(ref _shapeType, value, ShapeTypeSet); }
        [Export] public int ShapeSize { get => _shapeSize; set => this.Set(ref _shapeSize, Mathf.Max(0, value), ShapeSizeSet); }
        [Export] public int TerrainSize { get => _terrainSize; set => this.Set(ref _terrainSize, Mathf.Max(0, value), TerrainSizeSet); }
        [Export(PropertyHint.ColorNoAlpha)] public Color TerrainTint { get => _terrainTint; set => this.Set(ref _terrainTint, value, TerrainTintSet); }

        #endregion

        public event Action NoiseSet;
        public event Action AmplitudeSet;
        public event Action ShapeTypeSet;
        public event Action ShapeSizeSet;
        public event Action TerrainSizeSet;
        public event Action TerrainTintSet;

        public float GetHeight(float x, float z)
            => Amplitude * Noise?.GetNoise(x, z) ?? 1;
    }
}
