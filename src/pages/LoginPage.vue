<template>
  <div
    class="min-h-screen bg-gray-50 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8"
  >
    <div class="max-w-md w-full space-y-4">
      <div class="text-center flex flex-col items-center space-y-2">
        <RouterLink to="/">
          <img src="../assets/logo1.png" alt="Logo" class="h-20 w-20" />
        </RouterLink>
        <h2 class="text-2xl font-bold text-gray-900">Log In</h2>
      </div>

      <LoginForm />
    </div>
  </div>
</template>

<script setup lang="ts">
import LoginForm from "../components/LoginForm.vue"
</script>
