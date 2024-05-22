import Lenis from 'lenis'
import { env } from './Env'
import { OverlayScrollbars } from 'overlayscrollbars'

class Scroller {
  readonly lenis?: Lenis

  constructor() {
    if (!env.isTouch) {
      this.lenis = this.createLenis()
      this.createOverlayScrollbars()
    }
  }

  private createLenis() {
    const lenis = new Lenis()

    const raf = (time: number) => {
      lenis.raf(time)
      requestAnimationFrame(raf)
    }

    requestAnimationFrame(raf)

    return lenis
  }

  private createOverlayScrollbars() {
    return OverlayScrollbars(document.body, {
      overflow: { x: 'hidden' },
      scrollbars: { theme: 'os-theme-light', autoHide: 'scroll', autoHideDelay: 500, dragScroll: true },
    })
  }
}

export const scroller = new Scroller()
