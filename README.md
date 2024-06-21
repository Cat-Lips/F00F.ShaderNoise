# F00F.ShaderNoise
Syncronised noise for C# & Shaders

This project is a C# plugin (addon) for Godot.  It contains a trimmed down and slightly modified version of the [FastNoiseLite](https://github.com/Auburn/FastNoiseLite) library for [C#](https://github.com/Auburn/FastNoiseLite/blob/master/CSharp/FastNoiseLite.cs) and [glsl](https://github.com/Auburn/FastNoiseLite/blob/master/GLSL/FastNoiseLite.glsl) (renamed to .gdshaderinc).

It allows the same noise algorithm to be used on the GPU (shaders) and CPU (script) controlled by a single Godot Resource ([ShaderNoise2D](addons/GodotSharp.ShaderNoise/ShaderNoise2D.cs)).  See [Tests](Tests) for one potential use case.

Unfortunately, the current glsl implementation of FastNoiseLite has a large number of selection statements which can dramatically impact compilation time of godot shaders (experienced as initial startup time) and also run-time performance if [branching](https://shader-tutorial.dev/advanced/branching) is triggered.

To resolve this, all selection statements have been removed and, where possible, replaced with #defines.  OpenSimplex and Cellular noise had selection statements integral to their function, however Perlin noise was able to be retained.  Value/ValueCubic noise were removed in favour of a more minimal perlin-only implementation (and to minimise recompilation time when changing options) as well as all 3D noise.

The ShaderNoise2D resource maintains a dynamic ShaderMaterial, updated with the current #defines and code from the provided Shader object.  The ShaderMaterial can be assigned to any mesh or object as needed.  Property changes can be made via C# or Editor and will be reflected in the material and code at the same time.  Note that the ShaderMaterial need only be assigned once as it will automatically update as changes are made.  Note also, there might be a small delay when shader compilation is required (ie, when changing to a new enum/#define combination for the first time).

Feel free to create an issue for any problems or suggestions (especially if there is a better way of doing this!!!)

## Usage:
Add to [setup.sh](addons/setup.sh) for any project that requires this as a dependency.

1. Create a shader to include and use noise:
   - `#include "res://addons/F00F.ShaderNoise/FastNoiseLite.gdshaderinc"`
   - `get_noise(x, z)`
2. Create a script to include and use noise:
   - `[Export] private ShaderNoise2D MyNoise { get; set; }`
   - `MyNoise.GetNoise(x, z)`
3. Register Shader/ShaderMaterial with ShaderNoise2D:
   - If using Editor to assign shader material:
     - `MyNoise.ShaderMaterial = (ShaderMaterial)myMesh.MaterialOverride;`
   - If using C# script to assign shader material:
     - `MyNoise.Shader = GD.Load<Shader>("res://my_noise_shader.gdshader");`
     - `myMesh.MaterialOverride = Noise.ShaderMaterial;`
4. Edit noise properties via Editor or in-game UI (see extras)

## Extras:
For convenience and testing, or possibly even for in-game use, all F00F components come with a built-in set of edit controls.

Given a 2 column GridContainer, these edit controls can be added to set and track changes in their designated resource data object.
The only caveat is that if a new resource data object is required, it must also be registered with the edit controls via a conveniently provided callback method.

If no new object is assigned, one can set and forget in `_Ready()`:
```
public override void _Ready()
    => myNoise.AddEditControls(GetNode<GridContainer>("%Grid"));
```

Otherwise, one must call the provided SetData callback to register a new instance of the resource data object.
Assuming standard F00F usage, this can be done as follows:
```
public event Action NoiseSet;

public ShaderNoise2D Noise { get; set => this.Set(ref field, value, NoiseSet); }

public override void _Ready()
{
    var grid = GetNode<GridContainer>("%Grid");
    myNoise.AddEditControls(grid, out var SetNoiseData);
    NoiseSet += () => SetNoiseData(Noise);
}
```

There is also a static equivalent to create controls without the need for an existing resource data object:
```
ShaderNoise2D.GetEditControls(out var SetNoiseData));
```
