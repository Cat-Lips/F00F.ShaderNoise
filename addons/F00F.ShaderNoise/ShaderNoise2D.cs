using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using static F00F.ShaderNoise.FastNoiseLite;

namespace F00F.ShaderNoise
{
    [Tool, GlobalClass]
    public partial class ShaderNoise2D : Resource
    {
        #region Private

        private int mSeed = 0;
        private float mFrequency = 0.01f;
        private Vector2 mOffset = Vector2.Zero;
        private NoiseType mNoiseType = NoiseType.Perlin;

        private FractalType mFractalType = FractalType.None;
        private int mOctaves = 3;
        private float mLacunarity = 2.0f;
        private float mGain = 0.5f;
        private float mWeightedStrength = 0.0f;
        private float mPingPongStrength = 2.0f;

        private DomainWarp mDomainWarp = DomainWarp.None;
        private DomainWarpType mDomainWarpType = DomainWarpType.BasicGrid;
        private float mDomainWarpAmp = 1.0f;

        #endregion

        #region Export

        [Export] public int Seed { get => mSeed; set => this.Set(ref mSeed, value, SetShaderParam, fnl.SetSeed); }
        [Export(PropertyHint.Range, "0,1")] public float Frequency { get => mFrequency; set => this.Set(ref mFrequency, value, SetShaderParam, fnl.SetFrequency); }
        [Export] public Vector2 Offset { get => mOffset; set => this.Set(ref mOffset, value, SetShaderParam); }
        [Export] public NoiseType NoiseType { get => mNoiseType; set => this.Set(ref mNoiseType, value, fnl.SetNoiseType, ResetShaderCode.Run); }

        [ExportGroup("Fractal", "Fractal")]
        [Export] public FractalType FractalType { get => mFractalType; set => this.Set(ref mFractalType, value, notify: true, fnl.SetFractalType, ResetShaderCode.Run); }
        [ExportGroup("Fractal")]
        [Export(PropertyHint.Range, "1,10")] public int Octaves { get => mOctaves; set => this.Set(ref mOctaves, value, SetShaderParam, fnl.SetFractalOctaves); }
        [Export] public float Lacunarity { get => mLacunarity; set => this.Set(ref mLacunarity, value, SetShaderParam, fnl.SetFractalLacunarity); }
        [Export] public float Gain { get => mGain; set => this.Set(ref mGain, value, SetShaderParam, fnl.SetFractalGain); }
        [Export(PropertyHint.Range, "0,1")] public float WeightedStrength { get => mWeightedStrength; set => this.Set(ref mWeightedStrength, value, SetShaderParam, fnl.SetFractalWeightedStrength); }
        [Export] public float PingPongStrength { get => mPingPongStrength; set => this.Set(ref mPingPongStrength, value, SetShaderParam, fnl.SetFractalPingPongStrength); }

        [ExportGroup("DomainWarp", "DomainWarp")]
        [Export] public DomainWarp DomainWarp { get => mDomainWarp; set => this.Set(ref mDomainWarp, value, notify: true, fnl.SetDomainWarp, ResetShaderCode.Run); }
        [Export] public DomainWarpType DomainWarpType { get => mDomainWarpType; set => this.Set(ref mDomainWarpType, value, fnl.SetDomainWarpType, ResetShaderCode.Run); }
        [Export] public float DomainWarpAmp { get => mDomainWarpAmp; set => this.Set(ref mDomainWarpAmp, value, SetShaderParam, fnl.SetDomainWarpAmp); }

        #endregion

        public Shader Shader { get => _Shader; set => this.Set(ref _Shader, value, ResetShaderCode.Run); }
        public string ShaderCode { get => _ShaderCode; set => this.Set(ref _ShaderCode, value, ResetShaderCode.Run); }
        public ShaderMaterial ShaderMaterial { get => _ShaderMaterial; set => this.Set(ref _ShaderMaterial, value, ResetShaderCode.Run, ResetShaderParams.Run); }

        public float GetNoise(float x, float y)
            => fnl.GetNoise(x + Offset.X, y + Offset.Y);

        #region Private

        private Shader _Shader;
        private string _ShaderCode;
        private ShaderMaterial _ShaderMaterial = new() { Shader = new() };

        private readonly FastNoiseLite fnl = new();

        private AutoAction ResetShaderCode { get; } = new();
        private AutoAction ResetShaderParams { get; } = new();

        public ShaderNoise2D()
        {
            this.ResetShaderCode.Action += ResetShaderCode;
            this.ResetShaderParams.Action += ResetShaderParams;

            void ResetShaderCode()
            {
                if (ShaderMaterial is null) return;
                if (ShaderMaterial.Shader is null) return;

                ShaderMaterial.Shader.Code = string.Join("\n", Defs().Append(Code()));

                IEnumerable<string> Defs()
                {
                    yield return $"#define FNL_NOISE_{Enum.GetName(NoiseType).ToUpper()}";
                    yield return $"#define FNL_FRACTAL_{Enum.GetName(FractalType).ToUpper()}";
                    yield return $"#define FNL_DOMAIN_WARP_{Enum.GetName(DomainWarp).ToUpper()}";
                    yield return $"#define FNL_DOMAIN_WARP_{Enum.GetName(DomainWarpType).ToUpper()}";
                    yield return string.Empty;
                }

                string Code()
                    => ShaderCode ?? Shader?.Code;
            }

            void ResetShaderParams()
            {
                SetShaderParam(Seed);
                SetShaderParam(Frequency);
                SetShaderParam(Offset);

                SetShaderParam(Octaves);
                SetShaderParam(Lacunarity);
                SetShaderParam(Gain);
                SetShaderParam(WeightedStrength);
                SetShaderParam(PingPongStrength);

                SetShaderParam(DomainWarpAmp);

                void SetShaderParam<[MustBeVariant] T>(T _, [CallerArgumentExpression(nameof(_))] string member = null)
                    => this.SetShaderParam(member);
            }
        }

        private void SetShaderParam([CallerMemberName] string member = null)
            => ShaderMaterial.SetShaderParameter($"_fnl_{member.ToSnakeCase()}", Get(member));

        #endregion
    }
}
