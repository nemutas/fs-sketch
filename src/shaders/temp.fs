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

void main() {
  vec2 uv = vUv, asp = resolution / min(resolution.x, resolution.y), suv = (uv * 2.0 - 1.0) * asp;
  outColor = vec4(uv, 0.0, 1.0);
}

//----------------------------
// Reference
//----------------------------