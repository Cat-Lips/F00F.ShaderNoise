using System;
using System.Collections.Generic;
using System.Linq;
using F00F;
using Godot;

namespace Tests;

using ControlPair = (Control Label, Control EditControl);

public partial class TerrainData : IEditable<TerrainData>
{
    public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
    public IEnumerable<ControlPair> GetEditControls(out Action<TerrainData> SetData)
    {
        var ec = EditControls(out SetData);
        SetData(this);
        return ec;
    }

    public static IEnumerable<ControlPair> EditControls(out Action<TerrainData> SetTerrainData)
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
            ui.AddValue(nameof(Amplitude), @int: true);
            ui.AddOption(nameof(ShapeType), items: UI.Items<ShapeType>());
            ui.AddValue(nameof(ShapeSize), @int: true, range: (0, null, null));
            ui.AddValue(nameof(TerrainSize), @int: true, range: (0, null, null));
            ui.AddColor(nameof(TerrainTint));
            ui.AddSep();
        }
    }
}
