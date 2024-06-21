using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static F00F.FastNoiseLite.Const;

namespace F00F.ShaderNoise;

[Tool, GlobalClass]
public partial class PerlinNoise : CustomResource
{
    #region Export

    public class Const { public enum NoiseType { Noise2D, Noise3D } }
    [Export] public Const.NoiseType NoiseType { get; set => this.Set(ref field, value, notify: true, OnNoiseTypeSet); }

    [Export] public int Seed { get; set => this.Set(ref field, value, OnSeedSet); }
    [Export(PropertyHint.Range, "0,1")] public float Frequency { get; set => this.Set(ref field, value, OnFrequencySet); } = 0.01f;
    [Export] public Vector2 Offset2D { get; set => this.Set(ref field, value, OnOffset2DSet); }
    [Export] public Vector3 Offset3D { get; set => this.Set(ref field, value, OnOffset3DSet); }
    [Export] public Rotation3D Rotation3D { get; set => this.Set(ref field, value, OnRotation3DSet); }

    [ExportGroup("Fractal", "Fractal")]
    [Export] public FractalType FractalType { get; set => this.Set(ref field, value, notify: true, OnFractalTypeSet); }
    [ExportGroup("Fractal")]
    [Export(PropertyHint.Range, "1,10")] public int Octaves { get; set => this.Set(ref field, value, OnOctavesSet); } = 3;
    [Export] public float Lacunarity { get; set => this.Set(ref field, value, OnLacunaritySet); } = 2.0f;
    [Export] public float Gain { get; set => this.Set(ref field, value, OnGainSet); } = 0.5f;
    [Export(PropertyHint.Range, "0,1")] public float WeightedStrength { get; set => this.Set(ref field, value, OnWeightedStrengthSet); } = 0.0f;
    [Export] public float PingPongStrength { get; set => this.Set(ref field, value, OnPingPongStrengthSet); } = 2.0f;

    [ExportGroup("DomainWarp", "DomainWarp")]
    [Export] public DomainWarp DomainWarpType { get; set => this.Set(ref field, value, notify: true, OnDomainWarpTypeSet); }
    [Export] public float DomainWarpAmp { get; set => this.Set(ref field, value, OnDomainWarpAmpSet); } = 1.0f;

    #endregion

    public Shader Shader { get; set => this.Set(ref field, value, OnShaderSet); }
    public string ShaderCode { get; set => this.Set(ref field, value, OnShaderCodeSet); }
    public ShaderMaterial ShaderMaterial { get; set => this.Set(ref field, value ?? New.ShaderMaterial(), OnShaderMaterialSet); } = New.ShaderMaterial();

    public float GetNoise(float x, float y)
        => fnl.GetNoise(x + Offset2D.X, y + Offset2D.Y);

    public float GetNoise(float x, float y, float z)
        => fnl.GetNoise(x + Offset3D.X, y + Offset3D.Y, z + Offset3D.Z);

    #region Private

    private readonly FastNoiseLite fnl = new();

    private readonly AutoAction ResetShaderCode = new();
    private readonly AutoAction ResetShaderParams = new();

    public PerlinNoise()
    {
        this.ResetShaderCode.Run();
        this.ResetShaderParams.Run();
        this.ResetShaderCode.Action += ResetShaderCode;
        this.ResetShaderParams.Action += ResetShaderParams;

        void ResetShaderCode()
        {
            ShaderMaterial.Shader.Code = string.Join("\n", Defs().Append(Code()));

            IEnumerable<string> Defs()
            {
                yield return $"#define FNL_{Enum.GetName(NoiseType).ToSnakeCase().ToUpper()}";
                yield return $"#define FNL_FRACTAL_{Enum.GetName(FractalType).ToUpper()}";
                yield return $"#define FNL_DOMAIN_WARP_{Enum.GetName(DomainWarpType).ToUpper()}";
                yield return $"#define FNL_ROTATION_{Enum.GetName(Rotation3D).ToSnakeCase().ToUpper()}";
                yield return string.Empty;
            }

            string Code()
                => ShaderCode ?? Shader?.Code;
        }

        void ResetShaderParams()
        {
            OnGainSet();
            OnSeedSet();
            OnOctavesSet();
            OnOffset2DSet();
            OnOffset3DSet();
            OnFrequencySet();
            OnLacunaritySet();
            OnRotation3DSet();
            OnDomainWarpAmpSet();
            OnWeightedStrengthSet();
            OnPingPongStrengthSet();
        }
    }

    private void OnGainSet() => ShaderMaterial.SetShaderParameter("_fnl_gain", fnl.Gain = Gain);
    private void OnSeedSet() => ShaderMaterial.SetShaderParameter("_fnl_seed", fnl.Seed = Seed);
    private void OnOctavesSet() => ShaderMaterial.SetShaderParameter("_fnl_octaves", fnl.Octaves = Octaves);
    private void OnOffset2DSet() => ShaderMaterial.SetShaderParameter("_fnl_offset_xy", Offset2D);
    private void OnOffset3DSet() => ShaderMaterial.SetShaderParameter("_fnl_offset_xyz", Offset3D);
    private void OnFrequencySet() => ShaderMaterial.SetShaderParameter("_fnl_frequency", fnl.Frequency = Frequency);
    private void OnLacunaritySet() => ShaderMaterial.SetShaderParameter("_fnl_lacunarity", fnl.Lacunarity = Lacunarity);
    private void OnDomainWarpAmpSet() => ShaderMaterial.SetShaderParameter("_fnl_domain_warp_amp", fnl.DomainWarpAmp = DomainWarpAmp);
    private void OnWeightedStrengthSet() => ShaderMaterial.SetShaderParameter("_fnl_weighted_strength", fnl.WeightedStrength = WeightedStrength);
    private void OnPingPongStrengthSet() => ShaderMaterial.SetShaderParameter("_fnl_ping_pong_strength", fnl.PingPongStrength = PingPongStrength);

    private void OnNoiseTypeSet() => ResetShaderCode.Run();
    private void OnRotation3DSet() { fnl.Rotation3D = Rotation3D; ResetShaderCode.Run(); }
    private void OnFractalTypeSet() { fnl.FractalType = FractalType; ResetShaderCode.Run(); }
    private void OnDomainWarpTypeSet() { fnl.DomainWarpType = DomainWarpType; ResetShaderCode.Run(); }

    private void OnShaderSet() => ResetShaderCode.Run();
    private void OnShaderCodeSet() => ResetShaderCode.Run();
    private void OnShaderMaterialSet() { ResetShaderCode.Run(); ResetShaderParams.Run(); }

    #endregion
}
