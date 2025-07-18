import type { Request } from './types'
import { api } from '@/ky'
import { PagingModel } from '@/api/common/types'

export const getRequestsApi = (params: any): Promise<PagingModel<Request>> => {
    return api.get('http://localhost:5186/requests/', { searchParams: params, credentials: 'include' }).json()
}

export const saveRequestApi = async (data: Partial<Request>): Promise<boolean> => {
    if (data?.id) {
        const resp = await api.put(`http://localhost:5186/requests/${data.id}/`, { json: data, credentials: 'include' })
        return resp.status == 204
    }

    const resp = await api.post('http://localhost:5186/requests/', { json: data, credentials: 'include' })
    return resp.status == 201
}

export const getRequestApi = async (id: string): Promise<IResponse<Request>> => {
    return await api.get(`http://localhost:5186/requests/${id}/`, { credentials: 'include' }).json()
}

export const deleteRequestApi = async (ids: string[] | number[]): Promise<boolean> => {
    const resp = await api.delete(`http://localhost:5186/requests/${ids[0]}/`, { credentials: 'include' })
    return resp.status == 204
}

export const updateRequestStatusApi = async (id: string): Promise<boolean> => {
    const resp = await api.patch(`http://localhost:5186/requests/${id}/status`, {
        json: { status },
        credentials: 'include'
    });
    return resp.status === 204;
}