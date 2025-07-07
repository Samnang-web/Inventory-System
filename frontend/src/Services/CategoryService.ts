import type { Category } from "../Types/CategoryInterface";
import api from "./api";


export const getCategories = async (): Promise<Category[]> => {
    const response = await api.get<Category[]>('/Category');
    return response.data;
}

export const addCategories = async (category: Category): Promise<Category> =>{
    const response = await api.post('/Category', category);
    return response.data;
}
export const updateCategories = async (id: number, category: Category): Promise<any> => {
    const response = await api.put(`/Category/${id}`, category);
    return response.data;
}
export const deleteCategories = async (id: number): Promise<Category>=> {
    const response = await api.delete(`/Category/${id}`);
    return response.data;
}