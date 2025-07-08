import { api } from '@/ky'
import type { EnumValue } from './types'

export const getEnumValuesApi = async (type: string): Promise<EnumValue[]> => {
    return await api.get(`/api/enums/${type}/`).json()
}
