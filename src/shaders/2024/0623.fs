#version 300 es
precision highp float;

uniform sampler2D backBuffer;
uniform vec2 resolution;
uniform vec2 mouse;
uniform float time;
uniform float prevTime;
uniform int frame;
// uniform sampler2D textureUnit;
// uniform samplerCube cubeTextureUnit;
// uniform sampler2D singleText;

in vec2 vUv;
out vec4 outColor;

const float SEED = SEED_VALUE;

mat2 rot(float a) {
  float s = sin(a), c = cos(a);
  return mat2(c, s, -s, c);
}

// https://www.shadertoy.com/view/4sfGzS
float hash(ivec3 p) {
    // 3D -> 1D
  int n = p.x * 3 + p.y * 113 + p.z * 311;
    // 1D hash by Hugo Elias
  n = (n << 13) ^ n;
  n = n * (n * n * 15731 + 789221) + 1376312589;
  return float(n & ivec3(0x0fffffff)) / float(0x0fffffff);
}

float noise(in vec3 x) {
  ivec3 i = ivec3(floor(x));
  vec3 f = fract(x);
  f = f * f * (3.0 - 2.0 * f);
  return mix(mix(mix(hash(i + ivec3(0, 0, 0)), hash(i + ivec3(1, 0, 0)), f.x), mix(hash(i + ivec3(0, 1, 0)), hash(i + ivec3(1, 1, 0)), f.x), f.y), mix(mix(hash(i + ivec3(0, 0, 1)), hash(i + ivec3(1, 0, 1)), f.x), mix(hash(i + ivec3(0, 1, 1)), hash(i + ivec3(1, 1, 1)), f.x), f.y), f.z);
}

// https://www.shadertoy.com/view/XsGfWV
vec3 aces_tonemap(vec3 color) {	
	mat3 m1 = mat3(
        0.59719, 0.07600, 0.02840,
        0.35458, 0.90834, 0.13383,
        0.04823, 0.01566, 0.83777
	);
	mat3 m2 = mat3(
        1.60475, -0.10208, -0.00327,
        -0.53108,  1.10813, -0.07276,
        -0.07367, -0.00605,  1.07602
	);
	vec3 v = m1 * color;    
	vec3 a = v * (v + 0.0245786) - 0.000090537;
	vec3 b = v * (0.983729 * v + 0.4329510) + 0.238081;
	return pow(clamp(m2 * (a / b ), 0.0, 1.0), vec3(1.0 / 2.2));	
}

void main() {
  vec2 uv = vUv, asp = resolution / min(resolution.x, resolution.y), suv = (uv * 2.0 - 1.0) * asp;
  vec2 p = suv * 1.5;

  float fp = pow(0.5 / length(pow(abs(p * rot(0.45)) * vec2(2, 1), vec2(0.5))), 4.5);

  p *= mat2(0.7, -0.5, -0.4, 1.2);
  vec3 pos = normalize(vec3(p * rot(-0.4 / length(p)), 0.25));

  pos.z -= time * 2.0;

  vec3 q = 6.0 * pos;

  float f;
  f += 0.5000 * noise(q); q *= 2.0;
  f += 0.2500 * noise(q); q *= 2.0;
  f += 0.1250 * noise(q); q *= 2.0;
  f += 0.0625 * noise(q);

  float fr = 0.6 / length(p);
  f = smoothstep(-0.4, 2.0, f * f) * fr * fr + fp;

  vec2 n = uv * (1.0 - uv) * 3.0;
  float v = pow(n.x * n.y, 0.8);
  vec3 color = aces_tonemap(pow(f * f * vec3(2.0, 0.05, 0.0) * v, vec3(0.45)) * 3.5);

  // vec3 color = aces_tonemap(f * vec3(2.0, 0.40, 0.01));

  outColor = vec4(color, 1.0);
}

//----------------------------
// Reference
//----------------------------
// https://x.com/EKey2210/status/1769391406995435563
// https://neort.io/art/cnrgttcn70rtpq0hfkj0
// https://www.shadertoy.com/view/lXsSRN