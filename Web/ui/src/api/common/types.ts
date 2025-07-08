export type PagingModel<T> = {
    total: number
    data: T[]
}

export type PagingRequest = {
    pageIndex: number
    pageSize: number
    sort: string
}
