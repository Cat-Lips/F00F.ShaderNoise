using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F;

using FNL = ShaderNoise.FastNoiseLite;

[Tool, GlobalClass]
public partial class ShaderNoise2D : CustomResource
{
    #region Export

    [Export] public int Seed { get; set => this.Set(ref field, value, OnSeedSet); }
    [Export(PropertyHint.Range, "0,1")] public float Frequency { get; set => this.Set(ref field, value, OnFrequencySet); } = 0.01f;
    [Export] public Vector2 Offset { get; set => this.Set(ref field, value, OnOffsetSet); }

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
        => fnl.GetNoise(x + Offset.X, y + Offset.Y);

    #region Xtras

    private readonly HashSet<string> defs = [];
    public bool SetShaderDef(string def, bool enable = true)
    {
        return enable ? AddDef() : RemoveDef();

        bool AddDef()
        {
            if (defs.Add(def))
            {
                ResetShaderCode.Run();
                return true;
            }

            return false;
        }

        bool RemoveDef()
        {
            if (defs.Remove(def))
            {
                ResetShaderCode.Run();
                return true;
            }

            return false;
        }
    }

    private readonly Dictionary<string, string> defValues = [];
    public bool SetShaderDef<T>(string def, T value)
    {
        var newStr = $"{value}";
        if (!defValues.TryGetValue(def, out var oldStr))
        {
            defValues.Add(def, newStr);
            ResetShaderCode.Run();
            return true;
        }
        else if (newStr != oldStr)
        {
            defValues[def] = newStr;
            ResetShaderCode.Run();
            return true;
        }

        return false;
    }

    #endregion

    #region Private

    private readonly FNL fnl = new();

    public readonly AutoAction ResetShaderCode;
    private readonly AutoAction ResetShaderParams;

    public ShaderNoise2D()
    {
        (this.ResetShaderCode = new(ResetShaderCode)).Run();
        (this.ResetShaderParams = new(ResetShaderParams)).Run();

        void ResetShaderCode()
        {
            ShaderMaterial.Shader = new() { Code = string.Join("\n", MyDefs().Concat(XtraDefs()).Append(Code())) };

            IEnumerable<string> MyDefs()
            {
                yield return $"#define FNL_FRACTAL_{Enum.GetName(FractalType).ToUpper()}";
                yield return $"#define FNL_DOMAIN_WARP_{Enum.GetName(DomainWarpType).ToUpper()}";
                yield return string.Empty;
            }

            IEnumerable<string> XtraDefs()
            {
                if (defs.NotEmpty())
                {
                    foreach (var def in defs)
                        yield return $"#define {def}";
                    yield return string.Empty;
                }

                if (defValues.NotEmpty())
                {
                    foreach (var (def, value) in defValues)
                        yield return $"#define {def} {value}";
                    yield return string.Empty;
                }
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
            OnDomainWarpAmpSet();
            OnWeightedStrengthSet();
            OnPingPongStrengthSet();
        }
    }

    private void OnGainSet() => ShaderMaterial.SetShaderParameter("_fnl_gain", fnl.Gain = Gain);
    private void OnSeedSet() => ShaderMaterial.SetShaderParameter("_fnl_seed", fnl.Seed = Seed);
    private void OnOffsetSet() => ShaderMaterial.SetShaderParameter("_fnl_offset", Offset);
    private void OnOctavesSet() => ShaderMaterial.SetShaderParameter("_fnl_octaves", fnl.Octaves = Octaves);
    private void OnFrequencySet() => ShaderMaterial.SetShaderParameter("_fnl_frequency", fnl.Frequency = Frequency);
    private void OnLacunaritySet() => ShaderMaterial.SetShaderParameter("_fnl_lacunarity", fnl.Lacunarity = Lacunarity);
    private void OnDomainWarpAmpSet() => ShaderMaterial.SetShaderParameter("_fnl_domain_warp_amp", fnl.DomainWarpAmp = DomainWarpAmp);
    private void OnWeightedStrengthSet() => ShaderMaterial.SetShaderParameter("_fnl_weighted_strength", fnl.WeightedStrength = WeightedStrength);
    private void OnPingPongStrengthSet() => ShaderMaterial.SetShaderParameter("_fnl_ping_pong_strength", fnl.PingPongStrength = PingPongStrength);

    private void OnFractalTypeSet() { fnl.FractalType = FractalType; ResetShaderCode.Run(); }
    private void OnDomainWarpTypeSet() { fnl.DomainWarpType = DomainWarpType; ResetShaderCode.Run(); }

    private void OnShaderSet() { ResetShaderCode.Run(); Editor.TrackChanges(Shader, ResetShaderCode.Run); }
    private void OnShaderCodeSet() => ResetShaderCode.Run();

    private void OnShaderMaterialSet()
    {
        if (Shader is null && ShaderMaterial.Shader is not null)
            Shader = ShaderMaterial.Shader;

        ShaderMaterial.Shader = null;

        ResetShaderCode.Run();
        ResetShaderParams.Run();
    }

    #endregion
}
