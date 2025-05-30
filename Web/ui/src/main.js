import Vue from 'vue'

import 'normalize.css/normalize.css' // A modern alternative to CSS resets

import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'

import locale from 'element-ui/lib/locale/lang/ru-RU' 

import '@/styles/index.scss' // global css

import App from './App'
import store from './store'
import router from './router'
import request from '@/utils/request'
import * as utils from '@/utils'

import '@/icons' // icon
// import mock from '../mock'
import '@/permission' // permission control
import com from './components'
import { dictionaries, dictionaryTran } from '@/utils/dictionary'

Vue.use(ElementUI, { locale, size: 'small' })
Vue.use(com)

Vue.prototype.$dictionaries = dictionaries
Vue.prototype.$dictTran = dictionaryTran
Vue.prototype.$http = request
Vue.prototype.$utils = utils

Vue.config.productionTip = false

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})
