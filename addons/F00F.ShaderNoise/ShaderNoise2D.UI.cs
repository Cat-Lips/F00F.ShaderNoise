using System;
using System.Collections.Generic;
using Godot;
using static F00F.ShaderNoise.FastNoiseLite;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.ShaderNoise
{
    public partial class ShaderNoise2D
    {
        public IEnumerable<ControlPair> GetEditControls(bool all = true) => GetEditControls(out var _, all);
        public IEnumerable<ControlPair> GetEditControls(out Action<ShaderNoise2D> SetData, bool all = true)
        {
            var ec = EditControls(out SetData, all);
            SetData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<ShaderNoise2D> SetNoiseData, bool all = true)
        {
            return UI.Create(out SetNoiseData, CreateUI, CustomiseUI, !all ? HideOptionControls : null);

            static void CreateUI(UI.IBuilder ui)
            {
                ui.AddOption(nameof(NoiseType), "Sets noise algorithm", UI.Items<NoiseType>());
                ui.AddValue(nameof(Seed), "Sets seed used for all noise types", @int: true);
                ui.AddValue(nameof(Frequency), "Sets frequency for all noise types", range: (0, 1, .001f));
                ui.AddVec2(nameof(Offset), "Noise offset");
                ui.AddSep();
                ui.AddOption(nameof(FractalType), "Sets method for combining octaves in all fractal noise types", UI.Items<FractalType>());
                ui.AddValue(nameof(Octaves), "Sets octave count for all fractal noise types", @int: true, range: (1, 10, null));
                ui.AddValue(nameof(Lacunarity), "Sets octave lacunarity for all fractal noise types", range: (null, null, .01f));
                ui.AddValue(nameof(Gain), "Sets octave gain for all fractal noise types", range: (0, null, .1f));
                ui.AddValue(nameof(WeightedStrength), "Sets octave weighting for all fractal noise types", range: (0, 1, .1f));
                ui.AddValue(nameof(PingPongStrength), "Sets strength of the fractal ping pong effect", range: (0, null, .1f));
                ui.AddSep();
                ui.AddOption(nameof(DomainWarp), "Sets how to apply the Domain Warp algorithm", UI.Items<DomainWarp>());
                ui.AddOption(nameof(DomainWarpType), "Sets the warp algorithm when using DomainWarp", UI.Items<DomainWarpType>());
                ui.AddValue(nameof(DomainWarpAmp), "Sets the maximum warp distance (amplitude) from original position when using DomainWarp");
            }

            void CustomiseUI(UI.IBuilder ui)
            {
                var fractalController = ui.GetEditControl<OptionButton>(nameof(FractalType));
                var domainWarpController = ui.GetEditControl<OptionButton>(nameof(DomainWarp));

                var fractalControls = ui.GetControls(
                    nameof(Octaves),
                    nameof(Lacunarity),
                    nameof(Gain),
                    nameof(WeightedStrength));

                var pingPongControls = ui.GetControls(
                    nameof(PingPongStrength));

                var domainWarpControls = ui.GetControls(
                    nameof(DomainWarpType),
                    nameof(DomainWarpAmp));

                OnFractalChanged(default);
                OnDomainWarpChanged(default);
                fractalController.ItemSelected += OnFractalChanged;
                domainWarpController.ItemSelected += OnDomainWarpChanged;

                void OnFractalChanged(long index)
                {
                    var fractalType = (FractalType)index;
                    fractalControls.ForEach(x => x.Visible = fractalType is not FractalType.None);
                    pingPongControls.ForEach(x => x.Visible = fractalType is FractalType.PingPong);
                }

                void OnDomainWarpChanged(long index)
                {
                    var warpType = (DomainWarp)index;
                    domainWarpControls.ForEach(x => x.Visible = warpType is not DomainWarp.None);
                }
            }

            static void HideOptionControls(UI.IBuilder ui)
            {
                var optionControls = ui.GetControls(
                    nameof(NoiseType),
                    nameof(FractalType),
                    nameof(DomainWarp),
                    nameof(DomainWarpType));

                optionControls.ForEach(x => x.Visible = false);
            }
        }
    }
}
