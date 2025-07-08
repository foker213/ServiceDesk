import { EnumValue } from '@/api/enums/types'
import { EntityValue } from '@/api/entities/types'

export type Project = {
    id: number
    commercialOfferStage?: EnumValue
    commercialOfferState?: EnumValue
    requestDate: Date
    requestDescription: string
    engineerComment?: string
    note?: string
    objectName: string
    objectAddress: string
    clientName: string
    contactPerson: string
    responsibleManager: EntityValue
    mainEquipmentEngineer: EntityValue
    automationEngineer: EntityValue
    discount: number
    priceMarkup: number
    takeIntoVat: boolean
    currency: EnumValue
}
