import request from '@/utils/request'
import { getToken } from '@/utils/auth'

export function getList(params) {
  const token = getToken();
  return request({
    url: 'http://localhost:5186/Users/',
    method: 'get',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/x-www-form-urlencoded'
    },
    data: new URLSearchParams({
      pageSize: params.limit,
      pageIndex: params.page
    }).toString()
  });
}

export function deleteItem(id) {
  console.log('попытка удалить')
  return request({
    url: '/vue-admin-simple/table/delete',
    method: 'post',
    data: { id }
  })
}
