export type User = {
    id: number
    login: string
    email: string
    firstName: string
    lastName: string
    fullName: string
    phone: string
    blocked: boolean
    blockedReason: string
    blockedAt: Date
}
