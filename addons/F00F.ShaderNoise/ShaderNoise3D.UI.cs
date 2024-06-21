using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using static F00F.FastNoiseLite.Const;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.ShaderNoise;

public partial class ShaderNoise3D : IEditable<ShaderNoise3D>
{
    public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
    public IEnumerable<ControlPair> GetEditControls(out Action<ShaderNoise3D> SetData)
    {
        var ec = EditControls(out SetData);
        SetData(this);
        return ec;
    }

    public static IEnumerable<ControlPair> EditControls(out Action<ShaderNoise3D> SetNoiseData)
    {
        return UI.Create(out SetNoiseData, CreateUI, CustomiseUI, FixNames);

        static void CreateUI(UI.IBuilder ui)
        {
            ui.AddValue(nameof(Seed), TT.Seed, @int: true);
            ui.AddValue(nameof(Frequency), TT.Frequency, range: (0, 1, .001f));
            ui.AddVec3(nameof(Offset), TT.Offset);
            ui.AddOption(nameof(RotationType), TT.RotationType, UI.Items<Rotation3D>());
            ui.AddSep();
            ui.AddOption(nameof(FractalType), TT.FractalType, UI.Items<FractalType>());
            ui.AddValue(nameof(Octaves), TT.Octaves, @int: true, range: (1, 10, null));
            ui.AddValue(nameof(Lacunarity), TT.Lacunarity, range: (null, null, .01f));
            ui.AddValue(nameof(Gain), TT.Gain, range: (0, null, .1f));
            ui.AddValue(nameof(WeightedStrength), TT.WeightedStrength, range: (0, 1, .1f));
            ui.AddValue(nameof(PingPongStrength), TT.PingPongStrength, range: (0, null, .1f));
            ui.AddSep();
            ui.AddOption(nameof(DomainWarpType), TT.DomainWarpType, UI.Items<DomainWarp>());
            ui.AddValue(nameof(DomainWarpAmp), TT.DomainWarpAmp);
        }

        void CustomiseUI(UI.IBuilder ui)
        {
            var fractalType = ui.GetEditControl<OptionButton>(nameof(FractalType));
            var domainWarpType = ui.GetEditControl<OptionButton>(nameof(DomainWarpType));

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

            OnFractalTypeChanged(default);
            OnDomainWarpTypeChanged(default);

            fractalType.ItemSelected += OnFractalTypeChanged;
            domainWarpType.ItemSelected += OnDomainWarpTypeChanged;

            void OnFractalTypeChanged(long index)
            {
                var fractalType = (FractalType)index;
                fractalControls.ForEach(x => x.Visible = fractalType is not FractalType.None);
                pingPongControls.ForEach(x => x.Visible = fractalType is FractalType.PingPong);
            }

            void OnDomainWarpTypeChanged(long index)
            {
                var warpType = (DomainWarp)index;
                domainWarpControls.ForEach(x => x.Visible = warpType is not DomainWarp.None);
            }
        }

        void FixNames(UI.IBuilder ui)
        {
            var fractalType = ui.GetEditControl<OptionButton>(nameof(FractalType));
            var rotationType = ui.GetEditControl<OptionButton>(nameof(RotationType));

            // TODO:  Fix Capitalise
            Debug.Assert(fractalType.GetItemText(1) is "Fbm");
            Debug.Assert(rotationType.GetItemText(1) is "Improve Xy Planes");
            Debug.Assert(rotationType.GetItemText(2) is "Improve Xz Planes");

            fractalType.SetItemText(1, "FBm");
            rotationType.SetItemText(1, "Improve XY Planes");
            rotationType.SetItemText(2, "Improve XZ Planes");
        }
    }

    private class TT
    {
        public const string Seed = "The seed used for all noise types";
        public const string Frequency = "The frequency for all noise types";
        public const string Offset = "Noise offset for 3D noise";
        public const string RotationType = "The rotation type for 3D Noise and 3D DomainWarp (can aid in reducing directional artifacts when sampling a 2D plane in 3D)";
        public const string FractalType = "The method for combining octaves in all fractal noise types";
        public const string Octaves = "The octave count for all fractal noise types";
        public const string Lacunarity = "The octave lacunarity for all fractal noise types";
        public const string Gain = "The octave gain for all fractal noise types";
        public const string WeightedStrength = "The octave weighting for all fractal noise types";
        public const string PingPongStrength = "The strength of the fractal ping pong effect";
        public const string DomainWarpType = "The domain warp algorithm";
        public const string DomainWarpAmp = "The maximum warp distance from original position";
    }
}
