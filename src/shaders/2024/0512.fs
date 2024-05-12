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

#define si(a) (sin(a) * 0.5 + 0.5)

mat2 rot(float a) {
  float s = sin(a), c = cos(a);
  return mat2(c, s, -s, c);
}

vec3 hsv2rgb(vec3 c) {
  vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
  vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
  return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
  vec2 uv = vUv, asp = resolution / min(resolution.x, resolution.y), suv = (uv * 2.0 - 1.0) * asp;
  vec2 px = 1.0 / resolution;

  suv.x -= 0.5;
  if (1 < frame && suv.x < 0.0) {
    outColor = texture(backBuffer, uv + px * vec2(1.0, 0.0));
    return;
  }

  float scale = si(time) * (2.0 - 0.3) + 0.3;
  vec2 auv = abs(suv * rot(sin(time * 2.0) * PI * 0.5 + time));
  float th = 0.2 * scale, sm = 0.008;
  float c = smoothstep(th + sm, th, auv.x) * smoothstep(th + sm, th, auv.y);
  th *= (1.0 - 0.1 / scale);
  c -= smoothstep(th + sm, th, auv.x) * smoothstep(th + sm, th, auv.y);
  vec3 col = vec3(c);
  col *= hsv2rgb(vec3(si(time * 0.3), 0.5, 1.0));

  outColor = vec4(col, 1.0);
}

//----------------------------
// Reference
//----------------------------