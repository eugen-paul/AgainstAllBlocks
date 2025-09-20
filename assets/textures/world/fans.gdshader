shader_type spatial;
render_mode unshaded;

uniform sampler2D tex : source_color, filter_linear, repeat_enable;

// Steuerung
uniform int frame = 0;           // aktueller Frame (0 = sitzend, 1–2 = jubelnd)
uniform float speed = 2.0;       // Animation-Geschwindigkeit für Jubeln
uniform bool animate = false;    // true = Animation läuft, false = Frame manuell
uniform vec2 tiling = vec2(1.0, 1.0); // getrennte Skalierung für X und Y

void fragment() {
    vec2 uv = UV;

    // 3 Frames → jeder Frame ist 1/3 breit
    float frame_width = 1.0 / 3.0;
    int current_frame = frame;

    // Wenn Animation aktiv ist, automatisch zwischen Frame 1 und 2 wechseln
    if (animate) {
        current_frame = 1 + int(mod(floor(TIME * speed), 2.0));
    }

    // Offset berechnen
    uv.x = uv.x * frame_width + frame_width * float(current_frame);
    uv = uv * tiling;

    vec4 tex_color = texture(tex, uv);
    ALBEDO = tex_color.rgb;
    ALPHA = tex_color.a;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
