import { api } from '@/ky'
//import type { EntityValue } from './types'

export const getEntityValuesApi = async (
    entityName: string,
    sort: any
): Promise<{
    data: any[]
    total?: number
}> => {
    if (entityName.endsWith('a')) {
        entityName = entityName.substring(0, entityName.length - 1)
        entityName = entityName + 'e'
    }

    return await api.get(`/api/${entityName}s/`, { searchParams: { sort: sort } }).json()
}
