export interface Stock{
    id: number
    product_Id: number
    quantity: number
    types: string
    note: string
    created_by:number
    createdAt: Date
}

//export type StockForm = Omit<Stock, 'id' | 'createdAt'>
export interface StockFormDto{
    product_Id: number
    quantity: number
    types: string
    note: string
    created_by:number
}
