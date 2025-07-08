import type { UserType, UserLoginType } from './types'
import request from '@/axios'
import { api } from '@/ky'

interface RoleParams {
    roleName: string
}

export const loginApi = async (data: UserLoginType): Promise<any> => {
    const params = { useCookies: 'true' }
    const formData = new FormData()
    formData.append('username', data.username)
    formData.append('password', data.password)
    return await api.post('http://localhost:5186/auth/login', {
        searchParams: params,
        body: formData,
        credentials: 'include'
    })
}

export const loginOutApi = async (): Promise<any> => {
    return await api.post('http://localhost:5186/auth/logout', { credentials: 'include' })
}
export const getUserInfoApi = async (): Promise<UserType> => {
    return await api.get('http://localhost:5186/auth/UserInfo', { credentials: 'include' }).json()
}

export const getAdminRoleApi = (
    params: RoleParams
): Promise<IResponse<AppCustomRouteRecordRaw[]>> => {
    return request.get({ url: '/mock/role/list', params })
}

export const getTestRoleApi = (params: RoleParams): Promise<IResponse<string[]>> => {
    return request.get({ url: '/mock/role/list2', params })
}
