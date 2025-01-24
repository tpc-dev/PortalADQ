
import './src/assets/css/global.css'
import '@fontsource/roboto/100.css'
import '@fontsource/roboto/300.css'
import '@fontsource/roboto/400.css'
import '@fontsource/roboto/500.css'
import '@fontsource/roboto/700.css'
import '@fontsource/titillium-web/300.css'
import '@fontsource/titillium-web/400.css'
import '@fontsource/titillium-web/600.css'

export { wrapPageElement, wrapRootElement } from './src/providers/Wrap'

window.onbeforeunload = function () {
    sessionStorage.setItem('map', false)
}