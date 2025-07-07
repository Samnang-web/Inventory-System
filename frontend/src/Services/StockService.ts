import type { Stock, StockFormDto } from "../Types/StockInterface";
import api from "./api";


export const getStock = async (): Promise<Stock[]> => {
    const response = await api.get<Stock[]>('/Stock');
    return response.data;
}
export const addStock = async (stock: StockFormDto): Promise<any> => {
    const response = await api.post('/Stock', stock);
    return response.data;
}

export const updateStock = async (id: number, stock: StockFormDto) => {
    const response = await api.put(`/Stock/${id}`, stock)
    return response.data;
}
export const deleteStock = async (id: number): Promise<Stock> => {
    const response = await api.delete<Stock>(`/Stock/${id}`);
    return response.data;
}