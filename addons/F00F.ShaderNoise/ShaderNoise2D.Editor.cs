#if TOOLS
using Godot.Collections;
using static F00F.ShaderNoise.FastNoiseLite;

namespace F00F.ShaderNoise
{
    public partial class ShaderNoise2D
    {
        public override void _ValidateProperty(Dictionary property)
        {
            if (Editor.Hide(property, PropertyName.Gain, @if: FractalType is FractalType.None)) return;
            if (Editor.Hide(property, PropertyName.Octaves, @if: FractalType is FractalType.None)) return;
            if (Editor.Hide(property, PropertyName.Lacunarity, @if: FractalType is FractalType.None)) return;
            if (Editor.Hide(property, PropertyName.WeightedStrength, @if: FractalType is FractalType.None)) return;
            if (Editor.Show(property, PropertyName.PingPongStrength, @if: FractalType is FractalType.PingPong)) return;

            if (Editor.Hide(property, PropertyName.DomainWarpType, @if: DomainWarp is DomainWarp.None)) return;
            if (Editor.Hide(property, PropertyName.DomainWarpAmp, @if: DomainWarp is DomainWarp.None)) return;
        }
    }
}
#endif
