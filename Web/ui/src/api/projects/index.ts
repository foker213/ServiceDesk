import { api } from '@/ky'
import type { Project } from './types'
import { PagingModel } from '@/api/common/types'

export const getProjectsApi = (params: any): Promise<PagingModel<Project>> => {
    return api.get('/api/projects/', { searchParams: params }).json()
}

export const saveProjectApi = async (data: Partial<Project>): Promise<boolean> => {
    if (data?.id) {
        const resp = await api.put(`/api/projects/${data.id}/`, { json: data })
        return resp.status == 204
    }

    const resp = await api.post('/api/projects/', { json: data })
    return resp.status == 201
}

export const getProjectApi = async (id: string): Promise<IResponse<Project>> => {
    return await api.get(`/api/projects/${id}/`).json()
}

export const deleteProjectsApi = async (ids: string[] | number[]): Promise<boolean> => {
    if (ids.length == 1) {
        const resp = await api.delete(`/api/projects/${ids[0]}/`)
        return resp.status == 204
    }
    const resp = await api.delete(`/api/projects/`, { searchParams: { ids: ids.join(',') } })
    return resp.status == 204
}
