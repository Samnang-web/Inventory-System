export interface Category {
    id: number
    name: string
}

export type CategoryCreateDto = Omit<Category, 'id'>