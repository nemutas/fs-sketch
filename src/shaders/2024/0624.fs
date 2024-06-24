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

#define loop(n) for(int i=0;i<n;i++)

mat2 rot(float a) {
  float s = sin(a), c = cos(a);
  return mat2(c, s, -s, c);
}

vec3 cyc(vec3 p) {
  vec4 n;
  loop(4) {
    p += sin(p.yzx);
    n = 2.0 * n + vec4(cross(sin(p), cos(p.zxy)), 1.0);
    p *= 2.0;
  }
  return n.xyz / n.w;
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
  vec2 p = suv * 0.7;

  vec3 pos = normalize(vec3(p * rot(-0.4 / length(p)), 0.25));

  pos.z -= time * 0.2;

  vec3 q = 4.0 * pos;

  float f, h;
  f += 0.5000 * (h = cyc(q * 1.5 + h).x);
  f += 0.2500 * (h = cyc(q * 1.5 + h).x);
  f += 0.1250 * (h = cyc(q * 1.5 + h).x);
  f += 0.0625 * (h = cyc(q * 1.5 + h).x);

  float fr = 0.6 / length(p);
  f = smoothstep(-0.4, 2.0, f * f) * fr * fr * fr;

  vec3 color = aces_tonemap(f * vec3(2.0, 0.40, 0.01));

  outColor = vec4(color, 1.0);
}

//----------------------------
// Reference
//----------------------------
// https://x.com/EKey2210/status/1769391406995435563
// https://neort.io/art/cnrgttcn70rtpq0hfkj0
// https://www.shadertoy.com/view/lXsSRN