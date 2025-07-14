import { EnumValue } from '@/api/enums/types'

export type Request = {
    id: number
    userAttached?: string
    messageUnreadNotification: boolean
    dateStartRequest?: Date
    dateEndRequest?: Date
    description: string
    createdAt: Date
    updatedAt?: Date
    status: EnumValue
}
