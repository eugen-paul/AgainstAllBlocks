shader_type spatial;

uniform vec3 light_direction = vec3(0.3, 1.0, 0.0); // Simuliertes Licht von oben
uniform vec4 light_color = vec4(1.0, 0.0, 0.0, 1.0); // Lichtfarbe
uniform vec4 dark_color = vec4(0.5, 0.2, 0.2, 1.0); // Schattenfarbe

void fragment() {
    vec3 normal = normalize(NORMAL); // Normale des Meshes
    float light_intensity = max(dot(normal, light_direction), 0.0); // Berechne Lichtstärke

    vec4 color = mix(dark_color, light_color, light_intensity); // Mische zwischen Schatten- und Lichtfarbe
    ALBEDO = color.rgb;
}
