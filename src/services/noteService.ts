import api from "@/utils/axios"
import type { Note, NoteCreateUpdate } from "@/types/note"

export async function getNotes(): Promise<Note[]> {
  const { data } = await api.get<Note[]>("/Note/ViewNotes")
  return data
}

export async function getNote(id: string): Promise<Note> {
  const { data } = await api.get<Note>(`/Note/ViewNotesByID/${id}`)
  return data
}

export async function createNote(payload: NoteCreateUpdate): Promise<Note> {
  const { data } = await api.post<Note>("/Note/CreateNotes", payload)
  return data
}

export async function updateNote(
  id: string,
  payload: NoteCreateUpdate
): Promise<void> {
  await api.put(`/Note/UpdateNotes/${id}`, payload)
}

export async function deleteNote(id: string): Promise<void> {
  await api.delete(`/Note/DeleteNotes/${id}`)
}
