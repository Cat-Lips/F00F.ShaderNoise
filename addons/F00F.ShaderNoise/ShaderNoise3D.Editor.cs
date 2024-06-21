#if TOOLS
using Godot.Collections;
using static F00F.FastNoiseLite.Const;

namespace F00F.ShaderNoise;

public partial class ShaderNoise3D
{
    public override void _ValidateProperty(Dictionary property)
    {
        if (Editor.Hide(property, PropertyName.Gain, @if: FractalType is FractalType.None)) return;
        if (Editor.Hide(property, PropertyName.Octaves, @if: FractalType is FractalType.None)) return;
        if (Editor.Hide(property, PropertyName.Lacunarity, @if: FractalType is FractalType.None)) return;
        if (Editor.Hide(property, PropertyName.WeightedStrength, @if: FractalType is FractalType.None)) return;
        if (Editor.Show(property, PropertyName.PingPongStrength, @if: FractalType is FractalType.PingPong)) return;

        if (Editor.Hide(property, PropertyName.DomainWarpAmp, @if: DomainWarpType is DomainWarp.None)) return;
    }
}
#endif
