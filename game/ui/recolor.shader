shader_type spatial;
render_mode depth_draw_alpha_prepass;


uniform sampler2D block_texture: hint_albedo;

uniform vec4 recolored : hint_color;
uniform float alpha : hint_range(0, 1) = 1;

void fragment() {
	vec4 albedo = texture(block_texture, UV);
	ALPHA = alpha;
	ALBEDO = albedo.rgb - (1.0 - recolored.rgb) * recolored.a;
    // COLOR.rgb = vec3(1.0) - texture(TEXTURE, UV);
}