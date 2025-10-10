shader_type spatial;
render_mode vertex_lighting;

uniform sampler2D albedo_tex : source_color, filter_nearest, repeat_enable;
uniform float speed = 1.0;
uniform vec2 tiling = vec2(1.0, 1.0); // getrennte Skalierung für X und Y
uniform vec2 offset = vec2(0.0, 0.0); // getrennte Offset für X und Y

uniform sampler2D texture_emission : source_color, hint_default_black, filter_nearest, repeat_enable;
uniform vec4 emission : source_color;
uniform float emission_energy : hint_range(0.0, 100.0, 0.01);

void fragment() {
    vec2 uv = UV * tiling + offset;   // Skalierung der Textur
    uv.x += TIME * speed;    // Scrollen in Y-Richtung
    vec4 tex_color = texture(albedo_tex, uv);

    // Emission: Enabled
    vec3 emission_tex = texture(texture_emission, uv).rgb;
    // Emission Operator: Add
    EMISSION = (emission.rgb + emission_tex) * emission_energy;
    
    ALBEDO = tex_color.rgb;  // Oberflächenfarbe
    ALPHA  = tex_color.a;    // Transparenz, falls Textur Alpha hat
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
