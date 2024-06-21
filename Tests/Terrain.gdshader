shader_type spatial;

#include "res://addons/F00F.ShaderNoise/FastNoiseLite.gdshaderinc"

uniform float amplitude;
uniform vec3 base_albedo:source_color;

varying float noise_color;

// -- UTILITIES -- //

vec3 get_world_vertex(vec3 vertex, mat4 model_matrix) {
    return vertex + model_matrix[3].xyz;
}

// -- FUNCTIONS -- //

float get_noise(vec3 pos) {
#if defined (FNL_NOISE_2D)
    return get_noise_xy(pos.x, pos.z);
#elif defined (FNL_NOISE_3D)
    return get_noise_xyz(pos.x, pos.y, pos.z);
#else
    return 0.0; // Just to avoid error in initial load
#endif
}

// -- SHADERS -- //

void vertex() {
    vec3 noise_pos = get_world_vertex(VERTEX, MODEL_MATRIX).xyz;
    float noise_value = get_noise(noise_pos); // -1..1
    float terrain_height = noise_value * amplitude;

    VERTEX.y = terrain_height;

    noise_color = (noise_value + 1.0) * 0.5; // 0..1
}

void fragment() {
    ALBEDO = base_albedo * noise_color;
}
