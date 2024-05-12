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

in vec2 vUv;
out vec4 outColor;

const float SEED = SEED_VALUE;
const float PI = acos(-1.0);

mat2 rot(float a) {
  float s = sin(a), c = cos(a);
  return mat2(c, s, -s, c);
}

vec3 hash(vec3 v) {
  uvec3 x = floatBitsToUint(v + vec3(.1, .2, .3));
  x = (x >> 8 ^ x.yzx) * 0x456789ABu;
  x = (x >> 8 ^ x.yzx) * 0x6789AB45u;
  x = (x >> 8 ^ x.yzx) * 0x89AB4567u;
  return vec3(x) / vec3(-1u);
}

void main() {
  vec2 uv = vUv, asp = resolution / min(resolution.x, resolution.y), suv = (uv * 2.0 - 1.0) * asp;
  vec2 px = 1.0 / resolution;
  
  if (frame == 1) {
    vec2 auv = abs(suv);
    float c = step(auv.x, 0.5) * step(auv.y, 0.5);
    outColor = vec4(vec3(c), 1.0);
    return;
  }

  float lt = time * 120.0 / 60.0;
  float bt = floor(lt);
  float tt = tanh(fract(lt) * 5.0);
  lt = bt + tt;

  vec3 h = hash(vec3(bt + SEED));

  suv += floor(h.xy * asp * 5.0) / asp / 5.0 * 0.4 - 0.2;
  float angle = PI * (floor(h.z * 8.0) - 4.0) * 0.25;
  // float angle = PI * (floor(h.z * 4.0) - 2.0) * 0.5;
  suv *= rot(angle);

  vec2 offset = px * vec2(-1, 0) * rot(-angle) * 5.0 / asp;

  outColor = texture(backBuffer, uv + step(suv.y, 0.0) * offset);
}

//----------------------------
// Reference
//----------------------------