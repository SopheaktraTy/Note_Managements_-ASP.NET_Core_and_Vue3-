// src/utils/axios.ts
import axios from "axios"
import router from "../router"
import { getToken, clearToken } from "./authheader"

const api = axios.create({
  baseURL: "/api",
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json"
  }
})

api.interceptors.request.use(
  config => {
    const token = getToken()
    if (token) {
      config.headers = config.headers ?? {}
      ;(config.headers as any).Authorization = `Bearer ${token}`
    }
    return config
  },
  error => Promise.reject(error)
)

api.interceptors.response.use(
  res => res,
  async err => {
    const status = err?.response?.status
    if (status === 401 || status === 403) {
      clearToken()
      const current = router.currentRoute.value
      if (current?.name !== "login") {
        await router.push({
          name: "login",
          query: { redirect: current?.fullPath ?? "/" }
        })
      }
    }
    return Promise.reject(err)
  }
)

export default api
