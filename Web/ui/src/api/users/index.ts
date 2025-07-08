import { api } from '@/ky'
import type { User } from './types'
import { PagingModel } from '@/api/common/types'

export const getUsersApi = (params: any): Promise<PagingModel<User>> => {
    return api.get('http://localhost:5186/users/',
        {
            searchParams: params,
            credentials: 'include'
        }).json()
}

export const saveUserApi = async (data: Partial<User>): Promise<boolean> => {
    if (data?.id) {
        const resp = await api.put(`http://localhost:5186/users/${data.id}/`,
            {
                json: data,
                credentials: 'include'
            })
        return resp.status == 204
    }

    const resp = await api.post('http://localhost:5186/users/',
        {
            json: data,
            credentials: 'include'
        })
    return resp.status == 201
}

export const getUserApi = async (id: string): Promise<IResponse<User>> => {
    return await api.get(`http://localhost:5186/users/${id}/`,
        {
            credentials: 'include'
        }).json()
}

export const deleteUsersApi = async (ids: string[] | number[]): Promise<boolean> => {
    if (ids.length == 1) {
        const resp = await api.delete(`http://localhost:5186/users/${ids[0]}/`,
            {
                credentials: 'include'
            })
        return resp.status == 204
    }
    const resp = await api.delete(`http://localhost:5186/users/`,
        {
            searchParams: { ids: ids.join(',') },
            credentials: 'include'
        })
    return resp.status == 204
}
