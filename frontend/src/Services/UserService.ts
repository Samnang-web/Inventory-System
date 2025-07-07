import type { CreateDto, LoginDto, User } from "../Types/UserInterface";
import api from "./api";


export const Login = async (payload: LoginDto): Promise<User> => {
    const response = await api.post('User/login', payload);
    return response.data;
}
export const createUser = async (payload: CreateDto): Promise<User> =>{
    const response = await api.post('/User/register', payload);
    return response.data;
}
export const updateUser = async (id: number, payload: CreateDto): Promise<User> => {
  const response = await api.put(`User/${id}`, payload)
  return response.data
}

export const getUser = async (): Promise<User[]> => {
    const response = await api.get<User[]>('/User');
    return response.data;
}
export const deleteUserById = async (id: number): Promise<void> => {
  await api.delete(`User/${id}`)
}
