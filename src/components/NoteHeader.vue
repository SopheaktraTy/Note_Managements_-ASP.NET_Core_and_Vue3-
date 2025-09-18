<template>
  <header class="sticky top-0 z-50 bg-white/80 backdrop-blur border-b">
    <nav
      class="mx-auto max-w-10xl px-4 sm:px-6 lg:px-8 h-14 flex items-center justify-between"
    >
      <RouterLink to="/" class="flex items-center gap-2">
        <img
          v-if="logoExists"
          src="@/assets/logo.png"
          alt="Logo"
          class="h-8 w-auto select-none"
          draggable="false"
        />
      </RouterLink>

      <div class="flex items-center gap-3">
        <span v-if="userName" class="hidden sm:inline text-sm text-gray-600">
          {{ userName }}
        </span>

        <button
          v-if="isAuthed"
          @click="handleLogout"
          class="inline-flex items-center justify-center h-9 w-9 rounded-full hover:bg-gray-100 active:bg-gray-200 transition"
          title="Logout"
          aria-label="Logout"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            class="h-5 w-5 text-gray-700"
            fill="none"
            stroke="currentColor"
          >
            <path
              d="M9 6H6v12h3"
              stroke-width="1.8"
              stroke-linecap="round"
              stroke-linejoin="round"
            />

            <path d="M11 12h9" stroke-width="1.8" stroke-linecap="round" />

            <path
              d="M17 8l4 4-4 4"
              stroke-width="1.8"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </button>

        <RouterLink
          v-else
          to="/login"
          class="text-sm font-medium text-blue-600 hover:text-blue-700"
        >
          Login
        </RouterLink>
      </div>
    </nav>
  </header>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { useRouter } from "vue-router"
import { getToken, clearToken } from "@/utils/authheader"

const router = useRouter()

const isAuthed = computed(() => !!getToken())

const userName = computed(() => {
  try {
    const raw = localStorage.getItem("user")
    if (!raw) return ""
    const obj = JSON.parse(raw)
    const fn = obj?.firstname?.toString().trim()
    const ln = obj?.lastname?.toString().trim()
    return [fn, ln].filter(Boolean).join(" ")
  } catch {
    return ""
  }
})

function handleLogout() {
  clearToken()
  localStorage.removeItem("user")
  router.push({ name: "login" })
}

const logoExists = true
</script>
