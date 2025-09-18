import api from "@/utils/axios"
import { setToken, clearToken } from "@/utils/authheader"
import type {
  AuthLoginRequest,
  AuthRegisterRequest,
  AuthLoginResponse
} from "@/types/auth"

export async function login(
  payload: AuthLoginRequest
): Promise<AuthLoginResponse> {
  const { data } = await api.post<AuthLoginResponse>("/auth/login", payload)
  setToken(data.token)
  return data
}

export async function register(payload: AuthRegisterRequest): Promise<void> {
  await api.post("/auth/register", payload)
}

export function logout() {
  clearToken()
}
