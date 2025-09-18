<template>
  <div class="space-y-4 bg-white p-6 rounded-lg shadow-md">
    <div
      class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between"
    >
      <div class="flex flex-col sm:flex-row gap-3 sm:items-center">
        <div class="relative w-full sm:w-72">
          <svg
            class="absolute left-3 top-2.5 text-gray-400 h-4 w-4"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
          >
            <circle cx="11" cy="11" r="7" stroke-width="1.8" />
            <path d="M21 21l-3.5-3.5" stroke-width="1.8" />
          </svg>
          <input
            v-model="search"
            type="text"
            placeholder="Search notes..."
            class="w-full pl-9 pr-3 py-2 border rounded-md text-sm focus:outline-none focus:ring-1 focus:ring-gray-300"
          />
        </div>

        <select
          v-model="sortOption"
          class="w-full sm:w-56 py-2 px-3 border rounded-md text-sm focus:outline-none focus:ring-1 focus:ring-gray-300"
        >
          <option value="created_desc">Created • Newest</option>
          <option value="created_asc">Created • Oldest</option>
          <option value="updated_desc">Updated • Newest</option>
          <option value="updated_asc">Updated • Oldest</option>
          <option value="title_asc">Title • A → Z</option>
          <option value="title_desc">Title • Z → A</option>
        </select>
      </div>

      <button
        @click="openCreate()"
        class="bg-blue-600 text-white px-4 py-2 rounded-md flex items-center justify-center hover:bg-blue-700 transition-colors"
      >
        <svg
          class="w-4 h-4 mr-2"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
        >
          <path d="M12 5v14M5 12h14" stroke-width="2" stroke-linecap="round" />
        </svg>
        Create Note
      </button>
    </div>

    <div
      v-if="message"
      class="rounded-md bg-green-50 p-4 border border-green-200"
    >
      <div class="flex items-start">
        <svg
          class="h-5 w-5 text-green-500 mt-0.5 mr-3"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
        >
          <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14" stroke-width="1.8" />
          <path d="M22 4L12 14.01l-3-3" stroke-width="1.8" />
        </svg>
        <p class="text-sm font-medium text-green-800">{{ message }}</p>
      </div>
    </div>

    <div
      v-if="errorMessage"
      class="rounded-md bg-red-50 p-4 border border-red-200"
    >
      <div class="flex items-start">
        <svg
          class="h-5 w-5 text-red-500 mt-0.5 mr-3"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
        >
          <circle cx="12" cy="12" r="10" stroke-width="1.8" />
          <path
            d="M12 8v5M12 16h.01"
            stroke-width="1.8"
            stroke-linecap="round"
          />
        </svg>
        <p class="text-sm font-medium text-red-800">{{ errorMessage }}</p>
      </div>
    </div>

    <div class="overflow-x-auto border rounded-lg">
      <table class="min-w-full text-sm text-left">
        <thead class="bg-gray-100 text-gray-700">
          <tr>
            <th class="px-4 py-3 font-medium">Title</th>
            <th class="px-4 py-3 font-medium">Content</th>
            <th class="px-4 py-3 font-medium">Created</th>
            <th class="px-4 py-3 font-medium">Updated</th>
            <th class="px-4 py-3 text-center font-medium">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="note in paginatedNotes"
            :key="note.id"
            class="border-b hover:bg-gray-50 transition-colors"
          >
            <td class="px-4 py-3">{{ note.title }}</td>
            <td class="px-4 py-3">{{ truncate(note.content) }}</td>
            <td class="px-4 py-3 text-xs text-gray-500">
              {{ formatDate(note.createdAt) }}
            </td>
            <td class="px-4 py-3 text-xs text-gray-500">
              {{ formatDate(note.updatedAt) }}
            </td>
            <td class="px-4 py-3 text-center relative">
              <button
                class="p-2 rounded-full hover:bg-gray-100 focus:outline-none"
                aria-label="Open actions"
                @click.stop="openMenu($event, note)"
              >
                <svg
                  class="w-5 h-5 text-gray-600"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <circle cx="5" cy="12" r="2" />
                  <circle cx="12" cy="12" r="2" />
                  <circle cx="19" cy="12" r="2" />
                </svg>
              </button>
            </td>
          </tr>

          <tr v-if="paginatedNotes.length === 0">
            <td colspan="5" class="text-center py-8 text-gray-500">
              <div class="flex flex-col items-center gap-2">
                <svg
                  class="w-8 h-8 text-gray-300"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                >
                  <circle cx="11" cy="11" r="7" stroke-width="1.8" />
                  <path d="M21 21l-3.5-3.5" stroke-width="1.8" />
                </svg>
                <p>No notes found.</p>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div
      class="mt-6 flex flex-col sm:flex-row sm:items-center sm:justify-between text-sm text-gray-700"
    >
      <div class="flex items-center gap-2">
        <label for="itemsPerPage" class="whitespace-nowrap">Show</label>
        <select
          id="itemsPerPage"
          v-model.number="itemsPerPage"
          @change="currentPage = 1"
          class="border border-gray-300 rounded px-2 py-1 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors"
        >
          <option v-for="n in [5, 8, 10, 20]" :key="n" :value="n">
            {{ n }}
          </option>
        </select>
        <span class="whitespace-nowrap">per page</span>
      </div>

      <div class="flex items-center gap-4 mt-4 sm:mt-0">
        <span class="text-xs text-gray-500">
          {{ totalItems > 0 ? (currentPage - 1) * itemsPerPage + 1 : 0 }} -
          {{ Math.min(currentPage * itemsPerPage, totalItems) }}
          of {{ totalItems }}
        </span>

        <button
          aria-label="Previous Page"
          @click="currentPage--"
          :disabled="currentPage === 1"
          class="px-3 py-1 rounded border border-gray-300 disabled:text-gray-400 disabled:bg-gray-100 hover:bg-gray-100 transition-colors"
        >
          ←
        </button>

        <span
          class="text-xs font-normal px-3 py-2 text-gray-800 border-gray-300 bg-white rounded border"
        >
          {{ currentPage }} / {{ totalPages }}
        </span>

        <button
          aria-label="Next Page"
          @click="currentPage++"
          :disabled="currentPage === totalPages"
          class="px-3 py-1 rounded border border-gray-300 disabled:text-gray-400 disabled:bg-gray-100 hover:bg-gray-100 transition-colors"
        >
          →
        </button>
      </div>
    </div>

    <div
      v-if="isModalOpen"
      class="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4"
      @click.self="closeModal"
    >
      <div
        class="bg-white rounded-xl shadow-2xl w-full max-w-2xl max-h-[90vh] overflow-y-auto"
      >
        <div class="p-6">
          <div class="flex items-center justify-between mb-6">
            <h2 class="text-2xl font-bold text-gray-800">
              {{ editingId ? "Edit Note" : "Create Note" }}
            </h2>
            <button
              @click="closeModal"
              class="p-2 hover:bg-gray-100 rounded-full transition-colors"
            >
              <svg
                width="20"
                height="20"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
              >
                <path
                  d="M18 6L6 18M6 6l12 12"
                  stroke-width="2"
                  stroke-linecap="round"
                />
              </svg>
            </button>
          </div>

          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2"
                >Title *</label
              >
              <input
                v-model.trim="form.title"
                placeholder="Enter note title"
                class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 transition"
                required
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2"
                >Content</label
              >
              <textarea
                v-model.trim="form.content"
                rows="6"
                placeholder="Write your note..."
                class="appearance-none block w-full px-4 py-3 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 transition resize-vertical"
              />
              <p class="text-sm text-gray-500 mt-1">
                Current length: {{ form.content?.length || 0 }} characters
              </p>
            </div>

            <div class="flex justify-end gap-3 pt-6 border-t border-gray-200">
              <button
                @click="closeModal"
                class="px-6 py-3 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition-colors font-medium"
              >
                Cancel
              </button>
              <button
                @click="handleSubmit"
                class="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors font-medium"
              >
                {{ editingId ? "Update Note" : "Create Note" }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="menu.open"
      class="fixed z-50 w-40 bg-white border border-gray-200 rounded-lg shadow-lg note-menu-sentinel"
      :style="{ top: `${menu.y}px`, left: `${menu.x}px` }"
    >
      <button
        @click="menu.note && startEdit(menu.note)"
        class="w-full flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
      >
        <svg
          class="w-4 h-4 mr-2"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
        >
          <path d="M12 20h9" stroke-width="2" stroke-linecap="round" />
          <path
            d="M16.5 3.5a2.121 2.121 0 1 1 3 3L7 19l-4 1 1-4 12.5-12.5z"
            stroke-width="2"
            stroke-linejoin="round"
          />
        </svg>
        Edit
      </button>
      <button
        @click="menu.note && confirmDelete(menu.note.id)"
        class="w-full flex items-center px-4 py-2 text-sm text-red-700 hover:bg-red-50"
      >
        <svg
          class="w-4 h-4 mr-2"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
        >
          <path
            d="M3 6h18M8 6V4h8v2M6 6l1 14h10l1-14"
            stroke-width="2"
            stroke-linecap="round"
          />
        </svg>
        Delete
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onBeforeUnmount, reactive, ref, watch } from "vue"
import dayjs from "dayjs"
import {
  getNotes,
  createNote,
  updateNote,
  deleteNote
} from "@/services/noteService"
import type { Note, NoteCreateUpdate } from "@/types/note"

const notes = ref<Note[]>([])
const search = ref("")
type SortOption =
  | "created_desc"
  | "created_asc"
  | "updated_desc"
  | "updated_asc"
  | "title_asc"
  | "title_desc"
const sortOption = ref<SortOption>("created_desc")

const itemsPerPage = ref(5)
const currentPage = ref(1)
const message = ref("")
const errorMessage = ref("")

const isModalOpen = ref(false)
const editingId = ref<string | null>(null)
const form = reactive<NoteCreateUpdate>({ title: "", content: "" })

const menu = reactive<{
  open: boolean
  x: number
  y: number
  note: Note | null
}>({
  open: false,
  x: 0,
  y: 0,
  note: null
})

const filtered = computed(() => {
  const q = search.value.trim().toLowerCase()
  if (!q) return [...notes.value]
  return notes.value.filter(
    n =>
      (n.title || "").toLowerCase().includes(q) ||
      (n.content || "").toLowerCase().includes(q)
  )
})

const sorted = computed(() => {
  const arr = [...filtered.value]
  const by = sortOption.value
  arr.sort((a, b) => {
    const tA = (a.title || "").toLowerCase()
    const tB = (b.title || "").toLowerCase()
    const cA = new Date(a.createdAt || "").getTime() || 0
    const cB = new Date(b.createdAt || "").getTime() || 0
    const uA = new Date(a.updatedAt || "").getTime() || 0
    const uB = new Date(b.updatedAt || "").getTime() || 0

    if (by === "title_asc") return tA < tB ? -1 : tA > tB ? 1 : 0
    if (by === "title_desc") return tA > tB ? -1 : tA < tB ? 1 : 0
    if (by === "created_asc") return cA - cB
    if (by === "created_desc") return cB - cA
    if (by === "updated_asc") return uA - uB
    return uB - uA
  })
  return arr
})

const totalItems = computed(() => sorted.value.length)
const totalPages = computed(() =>
  Math.max(1, Math.ceil(totalItems.value / itemsPerPage.value))
)
const paginatedNotes = computed(() =>
  sorted.value.slice(
    (currentPage.value - 1) * itemsPerPage.value,
    currentPage.value * itemsPerPage.value
  )
)

watch([search, sortOption], () => {
  currentPage.value = 1
})
watch([message, errorMessage], ([m, e]) => {
  if (m || e)
    setTimeout(() => {
      message.value = ""
      errorMessage.value = ""
    }, 4000)
})

function openCreate() {
  editingId.value = null
  form.title = ""
  form.content = ""
  isModalOpen.value = true
}
function startEdit(n: Note) {
  editingId.value = n.id
  form.title = n.title
  form.content = n.content
  isModalOpen.value = true
  closeMenu()
}
function closeModal() {
  isModalOpen.value = false
}

function openMenu(e: MouseEvent, note: Note) {
  const r = (e.currentTarget as HTMLElement).getBoundingClientRect()
  const mw = 160,
    mh = 90
  let left = r.left + window.scrollX - (mw - r.width)
  let top = r.bottom + window.scrollY + 6
  const vw = window.innerWidth + window.scrollX
  const vh = window.innerHeight + window.scrollY
  if (left + mw > vw - 8) left = vw - mw - 8
  if (left < 8) left = 8
  if (top + mh > vh - 8) top = r.top + window.scrollY - mh - 6
  menu.open = true
  menu.x = Math.round(left)
  menu.y = Math.round(top)
  menu.note = note
  e.stopPropagation()
}
function closeMenu() {
  menu.open = false
  menu.note = null
}
function handleGlobalClick(ev: MouseEvent) {
  const t = ev.target as HTMLElement
  if (menu.open && !t.closest(".note-menu-sentinel")) closeMenu()
}
function handleScrollOrResize() {
  if (menu.open) closeMenu()
}

onMounted(() => {
  loadNotes()
  document.addEventListener("click", handleGlobalClick)
  window.addEventListener("scroll", handleScrollOrResize, true)
  window.addEventListener("resize", handleScrollOrResize)
})
onBeforeUnmount(() => {
  document.removeEventListener("click", handleGlobalClick)
  window.removeEventListener("scroll", handleScrollOrResize, true)
  window.removeEventListener("resize", handleScrollOrResize)
})

async function loadNotes() {
  try {
    notes.value = await getNotes()
  } catch {
    errorMessage.value = "Failed to load notes."
  }
}
async function handleSubmit() {
  if (!form.title.trim()) {
    errorMessage.value = "Title is required."
    return
  }
  try {
    if (editingId.value) {
      await updateNote(editingId.value, {
        title: form.title,
        content: form.content
      })
      message.value = "Note updated successfully."
    } else {
      await createNote({ title: form.title, content: form.content })
      message.value = "Note created successfully."
    }
    closeModal()
    await loadNotes()
  } catch {
    errorMessage.value = "Failed to save note."
  }
}
async function confirmDelete(id: string) {
  if (!confirm("Delete this note?")) return
  try {
    await deleteNote(id)
    message.value = "Note deleted successfully."
    await loadNotes()
  } catch {
    errorMessage.value = "Failed to delete note."
  } finally {
    closeMenu()
  }
}

function truncate(text = "", len = 80) {
  return text.length > len ? text.slice(0, len) + "…" : text
}
function formatDate(d?: string) {
  return d ? dayjs(d).format("MMM DD, YYYY") : "—"
}
</script>

<style scoped>
.note-menu-sentinel {
}
</style>
