<template>
  <NoteHeader />
  <div class="p-4">
    <NoteManagementList />
  </div>
</template>

<script setup lang="ts">
import NoteManagementList from "../components/NoteManagementList.vue"
import NoteHeader from "../components/NoteHeader.vue"
</script>
