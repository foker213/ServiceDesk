import { api } from '@/ky'
import type { User } from './types'
import { PagingModel } from '@/api/common/types'

export const getUsersApi = (params: any): Promise<PagingModel<User>> => {
    return api.get('http://localhost:5186/user/',
        {
            searchParams: params,
            credentials: 'include'
        }).json()
}

export const saveUserApi = async (data: Partial<User>): Promise<boolean> => {
    if (data?.id) {
        const resp = await api.put(`http://localhost:5186/user/${data.id}/`,
            {
                json: data,
                credentials: 'include'
            })
        return resp.status == 204
    }

    const resp = await api.post('http://localhost:5186/user/',
        {
            json: data,
            credentials: 'include'
        })
    return resp.status == 201
}

export const getUserApi = async (id: string): Promise<IResponse<User>> => {
    return await api.get(`http://localhost:5186/user/${id}/`,
        {
            credentials: 'include'
        }).json()
}

export const deleteUserApi = async (ids: string[] | number[]): Promise<boolean> => {
    const resp = await api.delete(`http://localhost:5186/user/${ids[0]}/`,
        {
            credentials: 'include'
        })
    return resp.status == 204
}
