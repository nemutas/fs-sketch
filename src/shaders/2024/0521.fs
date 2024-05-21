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
uniform sampler2D singleText;

in vec2 vUv;
out vec4 outColor;

const float SEED = SEED_VALUE;
const float PI = acos(-1.0);

vec3 hash(vec3 v) {
  uvec3 x = floatBitsToUint(v + vec3(0.1, 0.2, 0.3));
  x = (x >> 8 ^ x.yzx) * 0x456789ABu;
  x = (x >> 8 ^ x.yzx) * 0x6789AB45u;
  x = (x >> 8 ^ x.yzx) * 0x89AB4567u;
  return vec3(x) / vec3(-1u);
}

void main() {
  vec2 uv = vUv, asp = resolution / min(resolution.x, resolution.y), suv = (uv * 2.0 - 1.0) * asp;

  float n = 10.0;
  vec2 fuv = fract(uv * n * asp);
  vec2 iuv = floor(uv * n * asp) / n / asp;

  vec3 cellHash = hash(vec3(floor(uv * n * asp), SEED));

  vec2 tuv = iuv + fuv / n / asp;
  tuv = (tuv * 2.0 - 1.0) * asp;
  tuv = (tuv - vec2(0.0, 0.3)) * 0.5 + 0.5;

  float delay = cellHash.x * 2.0 - 1.0;
  tuv.x += sin(time * PI * 0.5 + delay * 0.5 * PI) * 0.05;

  vec3 col = texture(singleText, tuv).rgb;
  vec4 b = texture(backBuffer, uv);

  col = vec3(col.b, b.rg);
  col = mix(b.rgb, col, 0.2);

  outColor = vec4(col, 1.0);
}

//----------------------------
// Reference
//----------------------------