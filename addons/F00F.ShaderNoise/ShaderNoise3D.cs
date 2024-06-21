using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static F00F.FastNoiseLite.Const;

namespace F00F.ShaderNoise;

[Tool, GlobalClass]
public partial class ShaderNoise3D : CustomResource
{
    #region Export

    [Export] public int Seed { get; set => this.Set(ref field, value, OnSeedSet); }
    [Export(PropertyHint.Range, "0,1")] public float Frequency { get; set => this.Set(ref field, value, OnFrequencySet); } = 0.01f;
    [Export] public Vector3 Offset { get; set => this.Set(ref field, value, OnOffsetSet); }
    [Export] public Rotation3D RotationType { get; set => this.Set(ref field, value, OnRotationTypeSet); }

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

    public float GetNoise(float x, float y, float z)
        => fnl.GetNoise(x + Offset.X, y + Offset.Y, z + Offset.Z);

    #region Private

    private readonly FastNoiseLite fnl = new();

    private readonly AutoAction ResetShaderCode = new();
    private readonly AutoAction ResetShaderParams = new();

    public ShaderNoise3D()
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
                yield return $"#define FNL_FRACTAL_{Enum.GetName(FractalType).ToUpper()}";
                yield return $"#define FNL_DOMAIN_WARP_{Enum.GetName(DomainWarpType).ToUpper()}";
                yield return $"#define FNL_ROTATION_{Enum.GetName(RotationType).ToSnakeCase().ToUpper()}";
                yield return string.Empty;
            }

            string Code()
                => ShaderCode ?? Shader?.Code;
        }

        void ResetShaderParams()
        {
            OnGainSet();
            OnSeedSet();
            OnOffsetSet();
            OnOctavesSet();
            OnFrequencySet();
            OnLacunaritySet();
            OnRotationTypeSet();
            OnDomainWarpAmpSet();
            OnWeightedStrengthSet();
            OnPingPongStrengthSet();
        }
    }

    private void OnGainSet() => ShaderMaterial.SetShaderParameter("_fnl_gain", fnl.Gain = Gain);
    private void OnSeedSet() => ShaderMaterial.SetShaderParameter("_fnl_seed", fnl.Seed = Seed);
    private void OnOffsetSet() => ShaderMaterial.SetShaderParameter("_fnl_offset_xyz", Offset);
    private void OnOctavesSet() => ShaderMaterial.SetShaderParameter("_fnl_octaves", fnl.Octaves = Octaves);
    private void OnFrequencySet() => ShaderMaterial.SetShaderParameter("_fnl_frequency", fnl.Frequency = Frequency);
    private void OnLacunaritySet() => ShaderMaterial.SetShaderParameter("_fnl_lacunarity", fnl.Lacunarity = Lacunarity);
    private void OnDomainWarpAmpSet() => ShaderMaterial.SetShaderParameter("_fnl_domain_warp_amp", fnl.DomainWarpAmp = DomainWarpAmp);
    private void OnWeightedStrengthSet() => ShaderMaterial.SetShaderParameter("_fnl_weighted_strength", fnl.WeightedStrength = WeightedStrength);
    private void OnPingPongStrengthSet() => ShaderMaterial.SetShaderParameter("_fnl_ping_pong_strength", fnl.PingPongStrength = PingPongStrength);

    private void OnNoiseTypeSet() => ResetShaderCode.Run();
    private void OnFractalTypeSet() { fnl.FractalType = FractalType; ResetShaderCode.Run(); }
    private void OnRotationTypeSet() { fnl.Rotation3D = RotationType; ResetShaderCode.Run(); }
    private void OnDomainWarpTypeSet() { fnl.DomainWarpType = DomainWarpType; ResetShaderCode.Run(); }

    private void OnShaderSet() { ResetShaderCode.Run(); Editor.TrackChanges(Shader, ResetShaderCode.Run); }
    private void OnShaderCodeSet() => ResetShaderCode.Run();
    private void OnShaderMaterialSet() { ResetShaderCode.Run(); ResetShaderParams.Run(); }

    #endregion
}
