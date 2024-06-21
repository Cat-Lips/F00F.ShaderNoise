shader_type spatial;

#include "res://addons/F00F.ShaderNoise/FastNoiseLite.gdshaderinc"

uniform float amplitude;
uniform vec3 base_albedo:source_color;

varying float noise_color;

// -- UTILITIES -- //

vec3 get_world_vertex(vec3 vertex, mat4 model_matrix) {
    return vertex + model_matrix[3].xyz;
}

// -- SHADERS -- //

void vertex() {
    vec3 noise_pos = get_world_vertex(VERTEX, MODEL_MATRIX).xyz;
    float noise_value = get_noise(noise_pos.x, noise_pos.z); // -1..1
    float terrain_height = noise_value * amplitude;

    VERTEX.y = terrain_height;

    noise_color = (noise_value + 1.0) * 0.5; // 0..1
}

void fragment() {
    ALBEDO = base_albedo * noise_color;
}
