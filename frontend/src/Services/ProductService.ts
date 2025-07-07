import type { Product } from "../Types/ProductInterface";
import api from "./api";


export const getProduct = async (): Promise<Product[]> => {
    const response = await api.get<Product[]>('/Product')
    return response.data;
}
export const getProductById = async (id: number): Promise<Product> => {
    const response = await api.get<Product>(`/Product/${id}`);
    return response.data;
}
export const addProduct = async (formdata: FormData): Promise<any> => {
    const response = await api.post('/Product', formdata)
    return response.data;
}
export const updateProduct = async (id: number, formdata:FormData): Promise<any> => {
    const response = await api.put(`/Product/${id}`, formdata)
    return response.data;
}
export const deleteProduct = async (id: number): Promise<Product> => {
    const response = await api.delete<Product>(`/Product/${id}`);
    return response.data;
}