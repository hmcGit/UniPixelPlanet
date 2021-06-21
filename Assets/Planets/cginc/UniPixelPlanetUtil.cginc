#include "hlmod.cginc"

float _Pixels;
float _Size;
int _OCTAVES;
int _Seed;	
float _Time_speed;
float time;

float rand(float2 coord) {
	coord = mod(coord, float2(1.0,1.0)*round(_Size));
	return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 15.5453 * _Seed);
}

float noise(float2 coord){
	float2 i = floor(coord);
	float2 f = frac(coord);
	
	float a = rand(i);
	float b = rand(i + float2(1.0, 0.0));
	float c = rand(i + float2(0.0, 1.0));
	float d = rand(i + float2(1.0, 1.0));

	float2 cubic = f * f * (3.0 - 2.0 * f);

	return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
}

float fbm(float2 coord){
	float value = 0.0;
	float scale = 0.5;

	for(int i = 0; i < _OCTAVES ; i++){
		value += noise(coord) * scale;
		coord *= 2.0;
		scale *= 0.5;
	}
	return value;
}


// by Leukbaars from https://www.shadertoy.com/view/4tK3zR
float circleNoise(float2 uv) {
    float uv_y = floor(uv.y);
    uv.x += uv_y*.31;
    float2 f = frac(uv);
	float h = rand(float2(floor(uv.x),floor(uv_y)));
    float m = (length(f-0.25-(h*0.5)));
    float r = h*0.25;
    return smoothstep(0.0, r, m*0.75);
}

float cloud_alpha(float2 uv) {
	float c_noise = 0.0;
	
	// more iterations for more turbulence
	for (int i = 0; i < 9; i++) {
		c_noise += circleNoise((uv * _Size * 0.3) + (float(i+1)+10.) + (float2(time*_Time_speed, 0.0)));
	}
	float fbmval = fbm(uv*_Size+c_noise + float2(time*_Time_speed, 0.0));
	
	return fbmval;//step(a_cutoff, fbm);
}

bool dither(float2 uv_pixel, float2 uv_real) {
	return mod(uv_pixel.x+uv_real.y,2.0/_Pixels) <= 1.0 / _Pixels;
}

float2 spherify(float2 uv) {
	float2 centered= uv *2.0-1.0;
	float z = sqrt(1.0 - dot(centered.xy, centered.xy));
	float2 sphere = centered/(z + 1.0);
	return sphere * 0.5+0.5;
}

float2 rotate(float2 coord, float angle){
	coord -= 0.5;
	//coord *= float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));
    coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
	return coord + 0.5;
}