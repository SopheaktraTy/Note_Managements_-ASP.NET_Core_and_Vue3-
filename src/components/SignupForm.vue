<template>
  <div
    class="min-h-screen bg-gray-50 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8"
  >
    <div class="max-w-md w-full space-y-8">
      <div class="text-center flex flex-col items-center">
        <img src="../assets/logo1.png" alt="Logo" class="h-16 w-16 mb-4" />
        <h2 class="text-2xl font-bold text-gray-900">Sign Up</h2>
        <p class="mt-2 text-sm text-gray-600">
          Already have an account?
          <RouterLink
            to="/login"
            class="font-medium text-blue-600 hover:text-blue-500 underline focus:outline-none focus:ring-2 focus:ring-blue-500 rounded"
          >
            Sign in
          </RouterLink>
        </p>
      </div>

      <form
        @submit.prevent="handleSubmit"
        class="bg-white py-8 px-8 shadow-lg rounded-lg border border-gray-200 space-y-6"
        novalidate
      >
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label
              for="firstname"
              class="block text-sm font-medium text-gray-700 mb-1"
            >
              First Name
            </label>
            <input
              id="firstname"
              name="firstname"
              v-model.trim="form.firstname"
              placeholder="John"
              required
              class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
            />
          </div>
          <div>
            <label
              for="lastname"
              class="block text-sm font-medium text-gray-700 mb-1"
            >
              Last Name
            </label>
            <input
              id="lastname"
              name="lastname"
              v-model.trim="form.lastname"
              placeholder="Doe"
              required
              class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
            />
          </div>
        </div>

        <div>
          <label
            for="email"
            class="block text-sm font-medium text-gray-700 mb-1"
          >
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
          <label
            for="password"
            class="block text-sm font-medium text-gray-700 mb-1"
          >
            Password
          </label>
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

        <div>
          <label
            for="confirmPassword"
            class="block text-sm font-medium text-gray-700 mb-1"
          >
            Confirm Password
          </label>
          <div class="relative">
            <input
              id="confirmPassword"
              name="confirmPassword"
              :type="showConfirmPassword ? 'text' : 'password'"
              v-model.trim="form.confirmPassword"
              placeholder="Re-enter your password"
              required
              class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition pr-10"
            />
            <button
              type="button"
              @click="showConfirmPassword = !showConfirmPassword"
              class="absolute inset-y-0 right-0 flex items-center pr-3 text-gray-400 hover:text-gray-600 focus:outline-none"
              :aria-label="
                showConfirmPassword
                  ? 'Hide confirm password'
                  : 'Show confirm password'
              "
            >
              <svg
                v-if="!showConfirmPassword"
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

        <div class="flex items-center">
          <input
            id="acceptTerms"
            type="checkbox"
            v-model="acceptTerms"
            class="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500 focus:ring-2"
            required
          />
          <label
            for="acceptTerms"
            class="ml-2 block text-sm text-gray-700 select-none"
          >
            I accept the
            <a href="#" class="text-blue-600 hover:text-blue-500 underline"
              >Terms &amp; Conditions</a
            >
          </label>
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
          :disabled="loading || !acceptTerms"
          class="w-full flex justify-center py-3 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed transition"
        >
          {{ loading ? "Signing up..." : "Sign Up" }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue"
import { useRouter, useRoute } from "vue-router"
import { register } from "@/services/authService"
import type { AuthRegisterRequest } from "@/types/auth"

const router = useRouter()
const route = useRoute()

type FormState = AuthRegisterRequest & { confirmPassword: string }

const form = reactive<FormState>({
  firstname: "",
  lastname: "",
  email: "",
  password: "",
  confirmPassword: ""
})

const showPassword = ref(false)
const showConfirmPassword = ref(false)
const acceptTerms = ref(false)
const loading = ref(false)
const message = ref("")
const error = ref("")

async function handleSubmit() {
  error.value = ""
  message.value = ""

  if (form.password !== form.confirmPassword) {
    error.value = "Passwords do not match"
    return
  }

  loading.value = true
  try {
    const payload: AuthRegisterRequest = {
      firstname: form.firstname,
      lastname: form.lastname,
      email: form.email,
      password: form.password
    }

    await register(payload)
    message.value = "Account created successfully. You can now log in."
    const next = (route.query.redirect as string) || "/login"
    router.push(next)
    form.firstname = ""
    form.lastname = ""
    form.email = ""
    form.password = ""
    form.confirmPassword = ""
    acceptTerms.value = false
  } catch (e: any) {
    error.value = e?.response?.data?.message || e?.message || "Signup failed"
  } finally {
    loading.value = false
  }
}
</script>
