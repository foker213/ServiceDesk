import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import NProgress from 'nprogress' // progress bar
import { getToken } from '@/utils/auth'
import localDb from '@/utils/mockDb/localDb'
// create an axios instance
const service = axios.create({
  baseURL: import.meta.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 15000,
  adapter: (config) => {
    delete config.adapter
    const mock = localDb.mock.find(o => config.url.indexOf(o.url) > -1 && o.type === config.method.toLocaleLowerCase())
    if (mock) {
      console.info('Mock finished loading:', config.method, config.url)
      if (typeof config.data === 'string') {
        config.data = JSON.parse(config.data)
      }
      config.params = JSON.parse(JSON.stringify(config.params || '{}'))
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          resolve({ status: 200, data: mock.response(config) })
        }, 80)
      })
    }
    return axios(config)
  }
})

// request interceptor
service.interceptors.request.use(
  config => {
    store.state.loading = true
    NProgress.start() 
    console.log('req:', config.url, config.data || '', config.params)
    if (store.getters.token) {
      config.headers['X-Token'] = getToken()
    }
    return config
  },
  error => {
    console.log(error) 
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  response => {
    store.state.loading = false
    NProgress.done()
    const res = response.data
    if (response.status === 200) {
      return res
    } else {
      Message({ message: res.msg || 'Error', type: 'error' })
      return Promise.reject(new Error(res.msg || 'Error'))
    }
  },
  error => {
    store.state.loading = false
    NProgress.done()
    console.log('err' + error) // for debug
    Message({
      message: error.message,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
