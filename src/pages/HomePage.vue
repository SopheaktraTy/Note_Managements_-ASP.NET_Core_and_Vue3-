<template>
  <!-- full-screen hero -->
  <section class="relative w-full min-h-screen">
    <div
      class="relative min-h-screen overflow-hidden shadow-2xl ring-1 ring-black/5"
    >
      <!-- background image -->
      <img
        src="https://images.unsplash.com/photo-1469474968028-56623f02e42e?q=80&w=1920&auto=format&fit=crop"
        alt="Hero background"
        class="absolute inset-0 h-full w-full object-cover"
      />
      <!-- dark overlay -->
      <div class="absolute inset-0 bg-black/40"></div>

      <!-- top nav -->
      <header class="relative z-10">
        <nav class="flex items-center justify-between pl-10 pr-6 sm:px-8 pt-6">
          <!-- Brand -->
          <RouterLink to="/" class="flex items-center gap-2">
            <img
              :src="logoUrl"
              alt="Logo"
              class="h-9 w-auto select-none"
              draggable="false"
            />
          </RouterLink>

          <!-- Links (desktop) -->
          <ul class="hidden md:flex items-center gap-8 text-sm">
            <li><RouterLink class="nav" to="/">Home</RouterLink></li>
            <li><RouterLink class="nav" to="/about">About</RouterLink></li>
            <li><RouterLink class="nav" to="/features">Feature</RouterLink></li>
            <li><RouterLink class="nav" to="/contact">Contact</RouterLink></li>
          </ul>

          <!-- CTA -->
          <RouterLink
            to="/login"
            class="hidden md:inline-flex items-center rounded-full bg-pink-700 px-5 py-2 text-xs font-semibold text-white hover:bg-pink-800 transition"
          >
            Get started
          </RouterLink>

          <!-- Mobile CTA -->
          <RouterLink
            to="/login"
            class="md:hidden inline-flex items-center rounded-full bg-pink-700 px-4 py-2 text-xs font-semibold text-white hover:bg-pink-800 transition"
          >
            Start
          </RouterLink>
        </nav>
      </header>

      <!-- center hero copy -->
      <div
        class="relative z-10 flex items-center justify-center px-6 sm:px-10 pb-16 pt-20 sm:pt-24 md:pt-28 lg:pt-32"
        style="min-height: 60vh"
      >
        <div class="text-center text-white max-w-3xl">
          <h1 class="text-3xl sm:text-4xl md:text-5xl font-extrabold">
            Welcome
          </h1>
          <p
            class="mt-4 text-sm sm:text-base md:text-lg font-medium leading-relaxed text-white/95"
          >
            The new Note Management System streamlines how you capture,
            organize, and study your notes with advanced analysis and
            interactive features.
          </p>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import logoUrl from "@/assets/logo.png"
</script>

<style scoped>
.nav {
  @apply text-white/90 hover:text-white transition;
}
</style>
