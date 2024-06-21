using System;
using Godot;

namespace F00F.ShaderNoise.Tests
{
    [Tool]
    public partial class Terrain : Node3D
    {
        #region Private

        private bool ShapeDirty { get; set; } = true;

        #endregion

        #region Export

        private TerrainData _config;
        [Export] public TerrainData Config { get => _config; set => this.Set(ref _config, value, OnConfigSet, ConfigSet); }
        public event Action ConfigSet;

        [Export] private Camera3D Camera { get; set; }

        #endregion

        public MeshInstance3D Mesh => (MeshInstance3D)GetNode("Mesh");
        public CollisionShape3D Shape => (CollisionShape3D)GetNode("Shape");

        public float GetHeight(float x, float z)
        {
            return BaseHeight() + NoiseHeight();

            float BaseHeight() => ViewPos.Y;
            float NoiseHeight() => Config?.GetHeight(x, z) ?? 0;
        }

        #region Godot

        public override void _Ready()
            => Config ??= new();

        private Vector3 ViewPos { get; set; }
        public override void _Process(double delta)
        {
            if (Config is null) return;

            UpdateView();
            UpdateMesh();
            UpdateShape();

            void UpdateView()
            {
                if (Camera is null)
                {
                    SetViewPos(GlobalPosition);
                    return;
                }

                var camPos = Camera.GlobalPosition;
                var viewPos = camPos.RoundXZ(GlobalPosition.Y);
                //var viewPos = new Vector3(camPos.X, GlobalPosition.Y, camPos.Z);

                SetViewPos(viewPos);
                this.Clamp(Camera, Camera.Near * 2);

                void SetViewPos(in Vector3 viewPos)
                {
                    if (ViewPos != viewPos)
                    {
                        ViewPos = viewPos;
                        ShapeDirty = true;
                    }
                }
            }

            void UpdateMesh()
                => Mesh.GlobalPosition = ViewPos;

            void UpdateShape()
            {
                if (ShapeDirty)
                {
                    Shape.GlobalPosition = ViewPos;
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
                                var vertexPos = ViewPos + data[i];
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
                            var x0 = ViewPos.X - (width - 1) * .5f;
                            var z0 = ViewPos.Z - (depth - 1) * .5f;
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
            if (Config is null) return;

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
                        TestShaderInput();
                        TestShaderCodeInput();

                        // Use default noise material or provide your own
                        TestCustomMaterialOutput();
                        TestDefaultMaterialOutput();

                        Config.Noise.Changed += () => ShapeDirty = true;

                        ResetShaderParams();
                    }

                    void TestShaderInput() => Config.Noise.Shader = this.LoadShader();
                    void TestShaderCodeInput() => Config.Noise.ShaderCode = this.LoadShader().Code;
                    void TestCustomMaterialOutput() => Mesh.MaterialOverride = Config.Noise.ShaderMaterial = new ShaderMaterial { Shader = new() };
                    void TestDefaultMaterialOutput() => Mesh.MaterialOverride = Config.Noise.ShaderMaterial;
                    void TestNullNoise() => Mesh.MaterialOverride = new ShaderMaterial { Shader = new() };

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
}
