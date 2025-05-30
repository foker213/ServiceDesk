import request from '@/utils/request'

export function login(data) {
  return request({
    url: 'http://localhost:5186/Auth/Login',
    method: 'post',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded'
    },
    data: new URLSearchParams({
      username: data.username.trim(),
      password: data.password
    }).toString()
  })
}

export function getInfo(token) {
  return request({
    url: 'http://localhost:5186/Auth/UserInfo',
    method: 'get',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  })
}

export function logout(token) {
  return request({
    url: 'http://localhost:5186/Auth/Logout',
    method: 'post',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  })
}
