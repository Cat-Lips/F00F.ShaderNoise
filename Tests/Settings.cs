using System;
using System.Diagnostics;
using F00F;
using Godot;

namespace Tests;

[Tool]
public partial class Settings : DataView
{
    public TerrainData Data { get; set => this.Set(ref field, value, DataSet); }
    public event Action DataSet;

    private MenuButton ResetNoise => GetNode<MenuButton>("%ResetNoise");
    private Button ResetAll => GetNode<Button>("%ResetAll");

    protected override void OnReady()
    {
        InitData();
        InitGrid();
        InitReset();

        void InitData()
            => Data ??= new();

        void InitGrid()
        {
            Grid.Init(Data.GetEditControls(out var SetData));
            DataSet += () => SetData(Data);
        }

        void InitReset()
        {
            Debug.Assert(this.ResetNoise.GetPopup().ItemCount is 2);
            Debug.Assert(this.ResetNoise.GetPopup().GetItemText(0).EndsWith("Null"));
            Debug.Assert(this.ResetNoise.GetPopup().GetItemText(1).EndsWith("Default"));

            this.ResetNoise.GetPopup().IndexPressed += ResetNoise;
            this.ResetAll.Pressed += ResetAll;

            void ResetNoise(long idx)
                => Data.Noise = idx is 0 ? null : new();

            void ResetAll()
                => Data = new();
        }
    }
}
