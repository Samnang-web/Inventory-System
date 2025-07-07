export interface User {
  id?: number         
  username: string
  email: string
  password: string
  role: string
  token?: string   
}

export interface LoginDto {
  email: string
  password: string
}

export interface CreateDto {
  username: string
  email:string
  password: string
  role: string
}

