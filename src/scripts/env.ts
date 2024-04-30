class Env {
  readonly mobileMql
  readonly hoverableMql

  constructor() {
    this.mobileMql = matchMedia('(max-width: 768px)')
    this.hoverableMql = matchMedia('(hover: hover) and (pointer: fine)')
  }

  get isSp() {
    return this.mobileMql.matches
  }

  get isPc() {
    return !this.mobileMql.matches
  }

  get isTouch() {
    return !this.hoverableMql.matches
  }

  /**
   * @link https://developer.mozilla.org/ja/docs/Web/API/Window/navigator
   */
  get browser() {
    let sBrowser: 'Firefox' | 'Samsung' | 'Opera' | 'Explorer' | 'Edge (Legacy)' | 'Edge (Chromium)' | 'Chrome' | 'Safari' | 'Other'
    const sUsrAg = navigator.userAgent

    if (sUsrAg.indexOf('Firefox') > -1) sBrowser = 'Firefox'
    else if (sUsrAg.indexOf('SamsungBrowser') > -1) sBrowser = 'Samsung'
    else if (sUsrAg.indexOf('Opera') > -1 || sUsrAg.indexOf('OPR') > -1) sBrowser = 'Opera'
    else if (sUsrAg.indexOf('Trident') > -1) sBrowser = 'Explorer'
    else if (sUsrAg.indexOf('Edge') > -1) sBrowser = 'Edge (Legacy)'
    else if (sUsrAg.indexOf('Edg') > -1) sBrowser = 'Edge (Chromium)'
    else if (sUsrAg.indexOf('Chrome') > -1) sBrowser = 'Chrome'
    else if (sUsrAg.indexOf('Safari') > -1) sBrowser = 'Safari'
    else sBrowser = 'Other'

    return sBrowser
  }
}

export const env = new Env()
