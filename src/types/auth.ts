export interface AuthLoginRequest {
  email: string
  password: string
}

export interface AuthRegisterRequest {
  firstname: string
  lastname: string
  email: string
  password: string
}

export interface AuthLoginResponse {
  token: string
  firstname: string
  lastname: string
  email: string
}
