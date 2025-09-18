<template>
  <form
    @submit.prevent="handleSubmit"
    class="bg-white py-8 px-8 shadow-lg rounded-lg border border-gray-200 space-y-6"
  >
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700 mb-1">
        Email address
      </label>
      <input
        id="email"
        name="email"
        type="email"
        v-model.trim="form.email"
        placeholder="you@example.com"
        required
        class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
      />
    </div>
    <div>
      <div class="flex items-center justify-between mb-1">
        <label for="password" class="block text-sm font-medium text-gray-700">
          Password
        </label>
        <RouterLink
          to="/forgot-password"
          class="text-sm text-blue-600 hover:text-blue-500 underline focus:outline-none focus:ring-2 focus:ring-blue-500 rounded"
        >
          Forgot password?
        </RouterLink>
      </div>

      <div class="relative">
        <input
          id="password"
          name="password"
          :type="showPassword ? 'text' : 'password'"
          v-model.trim="form.password"
          placeholder="Enter your password"
          required
          class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition pr-10"
        />
        <button
          type="button"
          @click="showPassword = !showPassword"
          class="absolute inset-y-0 right-0 flex items-center pr-3 text-gray-400 hover:text-gray-600 focus:outline-none"
          :aria-label="showPassword ? 'Hide password' : 'Show password'"
        >
          <svg
            v-if="!showPassword"
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="1.8"
              d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12z"
            />
            <circle cx="12" cy="12" r="3" stroke-width="1.8" />
          </svg>
          <svg
            v-else
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="1.8"
              d="M3 3l18 18M10.6 10.7A3 3 0 0 0 12 15a3 3 0 0 0 3-3M9.9 4.3A10.8 10.8 0 0 1 12 4.1c6.5 0 10 7 10 7a18.6 18.6 0 0 1-3.3 4.3M6.1 6.2C3.8 7.9 2 11.1 2 11.1s3.5 7 10 7c1.2 0 2.3-.2 3.3-.6"
            />
          </svg>
        </button>
      </div>
    </div>
    <div
      v-if="message"
      class="mt-2 p-3 bg-green-50 border border-green-200 rounded-xl"
    >
      <p class="text-green-800 text-sm font-medium">{{ message }}</p>
    </div>
    <div
      v-if="error"
      class="mt-2 p-3 bg-red-50 border border-red-200 rounded-xl"
    >
      <p class="text-red-800 text-sm font-medium">{{ error }}</p>
    </div>
    <button
      type="submit"
      :disabled="loading"
      class="w-full flex justify-center py-3 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed transition"
    >
      {{ loading ? "Logging in..." : "Log In" }}
    </button>
    <p class="text-center text-sm text-gray-600">
      Don’t have an account?
      <RouterLink
        to="/signup"
        class="font-medium text-blue-600 hover:text-blue-500 underline"
        >Sign up</RouterLink
      >
    </p>
  </form>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue"
import { useRouter, useRoute } from "vue-router"
import type { AuthLoginRequest, AuthLoginResponse } from "@/types/auth"
import { login } from "@/services/authService"

const router = useRouter()
const route = useRoute()

const form = reactive<AuthLoginRequest>({ email: "", password: "" })
const showPassword = ref(false)
const loading = ref(false)
const message = ref("")
const error = ref("")

async function handleSubmit() {
  error.value = ""
  message.value = ""
  loading.value = true

  try {
    const res: AuthLoginResponse = await login(form)
    localStorage.setItem(
      "user",
      JSON.stringify({
        firstname: res.firstname,
        lastname: res.lastname,
        email: res.email
      })
    )

    message.value = "Login successful"

    const redirect = (route.query.redirect as string) || "/notes"
    router.push(redirect)
  } catch (e: any) {
    error.value = e?.response?.data?.message || e?.message || "Login failed"
  } finally {
    loading.value = false
  }
}
</script>
