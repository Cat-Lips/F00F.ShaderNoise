using System;
using System.Collections.Generic;
using System.Linq;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.ShaderNoise.Tests
{
    public partial class TerrainData
    {
        public IEnumerable<ControlPair> GetEditControls(bool all = true) => GetEditControls(out var _, all);
        public IEnumerable<ControlPair> GetEditControls(out Action<TerrainData> SetData, bool all = true)
        {
            var ec = EditControls(out SetData, all);
            SetData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<TerrainData> SetTerrainData, bool all = true)
        {
            var rootControls = UI.Create<TerrainData>(out var SetRootData, CreateUI);
            var noiseControls = ShaderNoise2D.EditControls(out var SetNoiseData);

            SetTerrainData = x =>
            {
                SetRootData(x);
                SetNoiseData(x?.Noise);
                if (x is null) return;
                x.NoiseSet += () => SetNoiseData(x.Noise);
            };

            return rootControls.Concat(noiseControls);

            static void CreateUI(UI.IBuilder ui)
            {
                ui.AddValue(nameof(Amplitude));
                ui.AddOption(nameof(ShapeType), items: UI.Items<ShapeType>());
                ui.AddValue(nameof(ShapeSize), @int: true, range: (0, null, null));
                ui.AddValue(nameof(TerrainSize), @int: true, range: (0, null, null));
                ui.AddColor(nameof(TerrainTint));
                ui.AddSep();
            }
        }
    }
}
