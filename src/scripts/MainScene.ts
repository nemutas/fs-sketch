import * as THREE from 'three'
import { mouse2d } from './common/Mouse2D'
import { BackBuffer } from './core/BackBuffer'
import { RawShaderMaterial } from './core/ExtendedMaterials'
import vertexShader from './shader/quad.vs'
import { params } from './common/Params'

export class MainScene extends BackBuffer {
  constructor(renderer: THREE.WebGLRenderer, fragmentShader: string) {
    fragmentShader = fragmentShader.replace('SEED_VALUE', Math.random().toFixed(5))

    const material = new RawShaderMaterial({
      uniforms: {
        backBuffer: { value: null },
        resolution: { value: [renderer.domElement.width * params.dpr, renderer.domElement.height * params.dpr] },
        mouse: { value: mouse2d.position },
        time: { value: 0 },
        prevTime: { value: 0 },
        frame: { value: 0 },
        singleText: { value: null },
      },
      vertexShader,
      fragmentShader,
      glslVersion: '300 es',
    })

    super(renderer, material, {
      renderTargetOptions: {
        type: THREE.UnsignedByteType,
        generateMipmaps: params.filterType === THREE.LinearFilter,
        minFilter: params.filterType,
        magFilter: params.filterType,
        wrapS: THREE.RepeatWrapping,
        wrapT: THREE.RepeatWrapping,
      },
    })

    if (params.singleText) {
      this.uniforms.singleText.value = this.createSingleTextTexture(params.singleText.text, params.singleText.dpr)
    }
  }

  private createSingleTextTexture(text: string, dpr: number) {
    const canvas = document.createElement('canvas')
    canvas.width = Math.min(window.innerWidth * dpr, window.innerHeight * dpr)
    canvas.height = canvas.width
    const ctx = canvas.getContext('2d')!

    const scale = 0.8
    ctx.font = `400 ${canvas.height * scale}px serif`
    ctx.fillStyle = '#fff'
    const measure = ctx.measureText(text)
    const x = (canvas.width - measure.width) / 2
    const y = canvas.height - measure.actualBoundingBoxDescent - (measure.fontBoundingBoxDescent / 2) * scale
    ctx.fillText(text, x, y)

    const texture = new THREE.CanvasTexture(canvas)
    texture.generateMipmaps = false

    return texture
  }

  resize() {
    super.resize()
    this.uniforms.resolution.value = [this.size.width, this.size.height]
    this.uniforms.time.value = 0
    this.uniforms.prevTime.value = 0
    this.uniforms.frame.value = 0
  }

  render(dt: number) {
    this.uniforms.backBuffer.value = this.backBuffer
    this.uniforms.mouse.value = mouse2d.position
    this.uniforms.prevTime.value = this.uniforms.time.value
    this.uniforms.time.value += dt
    this.uniforms.frame.value += 1
    super.render()
  }
}
