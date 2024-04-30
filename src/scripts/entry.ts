import { Canvas } from './Canvas'

async function loadShader(y: string, md: string) {
  let fs: string | undefined, fs_output: string | undefined
  try {
    fs = (await import(`../shaders/${y}/${md}.fs`)).default
  } catch {
    fs = undefined
  }
  try {
    fs_output = (await import(`../shaders/${y}/${md}_output.fs`)).default
  } catch {
    fs_output = undefined
  }
  return { fs, fs_output }
}

async function entry() {
  const urlFragments = location.href.split('/')
  const md = urlFragments.at(-2)
  const y = urlFragments.at(-3)
  if (!y || !md) return

  const titleEl = document.querySelector<HTMLTitleElement>('title')!
  if (!titleEl.innerText) titleEl.innerText = y + md

  const shader = await loadShader(y, md)
  if (!shader.fs) return

  new Canvas(document.querySelector<HTMLCanvasElement>('canvas')!, shader.fs, shader.fs_output)
  logShaderLink(y, md)
}

function logShaderLink(y: string, md: string) {
  console.log(`https://github.com/nemutas/fs-sketch/blob/main/src/shaders/${y}/${md}.fs`)
}

entry()
