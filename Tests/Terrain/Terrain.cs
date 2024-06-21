using F00F;
using Godot;

namespace Tests;

[Tool]
public partial class Terrain : StaticBody3D
{
    #region Private

    private bool ShapeDirty { get; set; } = true;
    private Godot.Camera3D Camera => field ??= GetViewport().GetCamera3D();

    #endregion

    #region Export

    [Export] public TerrainData Config { get; set => this.Set(ref field, value ?? new(), OnConfigSet); }

    #endregion

    public MeshInstance3D Mesh => field ??= GetNode<MeshInstance3D>("Mesh");
    public CollisionShape3D Shape => field ??= GetNode<CollisionShape3D>("Shape");

    public float GetHeight(float x, float z)
    {
        return BaseHeight() + NoiseHeight();

        float BaseHeight() => CurPos.Y;
        float NoiseHeight() => Config?.GetHeight(x, z) ?? 0;
    }

    #region Godot

    public sealed override void _Ready()
        => Config ??= new();

    private Vector3 CurPos { get; set; }
    public sealed override void _Process(double delta)
    {
        UpdateView();
        UpdateMesh();
        UpdateShape();

        void UpdateView()
        {
            if (Camera is null) return;

            var camPos = Camera.GlobalPosition;
            var curPos = camPos.RoundXZ(GlobalPosition.Y);

            SetCurrentPosition(curPos);
            this.Clamp(Camera, Camera.Near * 2);

            void SetCurrentPosition(in Vector3 curPos)
            {
                if (CurPos != curPos)
                {
                    CurPos = curPos;
                    ShapeDirty = true;
                }
            }
        }

        void UpdateMesh()
            => Mesh.GlobalPosition = CurPos;

        void UpdateShape()
        {
            if (ShapeDirty)
            {
                Shape.GlobalPosition = CurPos;
                ShapeDirty = false;
                UpdateShape();
            }

            void UpdateShape()
            {
                switch (Config.ShapeType)
                {
                    case ShapeType.Polygon:
                        UpdatePolygonShape((ConcavePolygonShape3D)Shape.Shape);
                        break;
                    case ShapeType.HeightMap:
                        UpdateHeightMapShape((HeightMapShape3D)Shape.Shape);
                        break;
                }

                void UpdatePolygonShape(ConcavePolygonShape3D shape)
                {
                    var data = shape.Data;
                    UpdatePolygonShape();
                    shape.Data = data;

                    void UpdatePolygonShape()
                    {
                        for (var i = 0; i < data.Length; ++i)
                        {
                            var vertexPos = CurPos + data[i];
                            data[i].Y = GetHeight(vertexPos.X, vertexPos.Z);
                        }
                    }
                }

                void UpdateHeightMapShape(HeightMapShape3D shape)
                {
                    var data = shape.MapData;
                    UpdateHeightMapShape(shape.MapWidth, shape.MapDepth);
                    shape.MapData = data;

                    void UpdateHeightMapShape(int width, int depth)
                    {
                        var x0 = CurPos.X - (width - 1) * .5f;
                        var z0 = CurPos.Z - (depth - 1) * .5f;
                        for (var x = 0; x < width; ++x)
                        {
                            for (var z = 0; z < depth; ++z)
                            {
                                var i = x + z * width;
                                data[i] = GetHeight(x0 + x, z0 + z);
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Private

    private void OnConfigSet()
    {
        InitMesh();
        InitShape();
        InitShader();
        InitMeshCullMargin();
        InitShaderAmplitude();
        InitShaderBaseAlbedo();

        void InitMesh()
        {
            ResetMesh();
            Config.TerrainSizeSet += ResetMesh;

            void ResetMesh()
            {
                ShapeDirty = true;
                Mesh.Mesh = CreateMesh(Config.TerrainSize);

                static PlaneMesh CreateMesh(int size) => new()
                {
                    Size = Vector2.One * size,
                    SubdivideDepth = size - 1,
                    SubdivideWidth = size - 1,
                };
            }
        }

        void InitShape()
        {
            ResetShape();
            Config.ShapeTypeSet += ResetShape;
            Config.ShapeSizeSet += ResetShape;

            void ResetShape()
            {
                ShapeDirty = true;
                switch (Config.ShapeType)
                {
                    case ShapeType.Polygon:
                        Shape.Shape = CreatePolygonShape(Config.ShapeSize);
                        break;
                    case ShapeType.HeightMap:
                        Shape.Shape = CreateHeightMapShape(Config.ShapeSize);
                        break;
                }

                static ConcavePolygonShape3D CreatePolygonShape(int size)
                {
                    return new() { Data = CreateVertices(size) };

                    static Vector3[] CreateVertices(int size) => new PlaneMesh
                    {
                        Size = Vector2.One * size,
                        SubdivideWidth = size - 1,
                        SubdivideDepth = size - 1,
                    }.GetFaces();
                }

                static HeightMapShape3D CreateHeightMapShape(int size) => new()
                {
                    MapDepth = size + 1,
                    MapWidth = size + 1,
                };
            }
        }

        void InitShader()
        {
            ResetShader();
            Config.NoiseSet += ResetShader;

            void ResetShader()
            {
                ShapeDirty = true;
                if (Config.Noise is null)
                {
                    TestNullNoise();
                }
                else
                {
                    // Assign Shader or use ShaderCode if custom code required
                    TestShaderInput(); // Choose one of these
                    TestShaderCodeInput();

                    // Use default noise material or provide your own
                    TestCustomMaterialOutput(); // Choose one of these
                    TestDefaultMaterialOutput();

                    Config.Noise.Changed += () => ShapeDirty = true;

                    ResetShaderParams();
                }

                void TestShaderInput() => Config.Noise.Shader = this.LoadShader();
                void TestShaderCodeInput() => Config.Noise.ShaderCode = this.LoadShader().Code;
                void TestCustomMaterialOutput() => Mesh.MaterialOverride = Config.Noise.ShaderMaterial = New.ShaderMaterial();
                void TestDefaultMaterialOutput() => Mesh.MaterialOverride = Config.Noise.ShaderMaterial;
                void TestNullNoise() => Mesh.MaterialOverride = New.ShaderMaterial();

                void ResetShaderParams()
                {
                    var shader = (ShaderMaterial)Mesh.MaterialOverride;
                    shader.SetShaderParameter("amplitude", Config.Amplitude);
                    shader.SetShaderParameter("base_albedo", Config.TerrainTint);
                }
            }
        }

        void InitMeshCullMargin()
        {
            SetMeshCullMargin();
            Config.AmplitudeSet += SetMeshCullMargin;
            Config.TerrainSizeSet += SetMeshCullMargin;

            void SetMeshCullMargin()
                => Mesh.ExtraCullMargin = Mathf.Max(Config.TerrainSize, Config.Amplitude) * .5f;
        }

        void InitShaderAmplitude()
        {
            SetShaderAmplitude();
            Config.AmplitudeSet += SetShaderAmplitude;

            void SetShaderAmplitude()
            {
                ShapeDirty = true;
                ((ShaderMaterial)Mesh.MaterialOverride).SetShaderParameter("amplitude", Config.Amplitude);
            }
        }

        void InitShaderBaseAlbedo()
        {
            SetShaderBaseAlbedo();
            Config.TerrainTintSet += SetShaderBaseAlbedo;

            void SetShaderBaseAlbedo()
                => ((ShaderMaterial)Mesh.MaterialOverride).SetShaderParameter("base_albedo", Config.TerrainTint);
        }
    }

    #endregion
}
