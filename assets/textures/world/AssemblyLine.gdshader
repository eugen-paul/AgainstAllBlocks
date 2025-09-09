shader_type spatial;
render_mode unshaded;

uniform sampler2D albedo_tex : source_color, filter_linear, repeat_enable;
uniform float speed = 1.0;
uniform vec2 tiling = vec2(1.0, 1.0); // getrennte Skalierung für X und Y

void fragment() {
    vec2 uv = UV * tiling;   // Skalierung der Textur
    uv.x += TIME * speed;    // Scrollen in Y-Richtung
    vec4 tex_color = texture(albedo_tex, uv);

    ALBEDO = tex_color.rgb;  // Oberflächenfarbe
    ALPHA  = tex_color.a;    // Transparenz, falls Textur Alpha hat
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
