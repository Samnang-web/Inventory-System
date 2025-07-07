<template>
  <div class="p-6 bg-white rounded shadow max-w-5xl mx-auto mt-10">
    <h1 class="text-3xl font-bold mb-6">Users</h1>
    <button
      @click="openModal()"
      class="mb-6 px-5 py-2 bg-purple-600 text-white rounded hover:bg-purple-700"
    >
      Add User
    </button>

    <table class="min-w-full table-auto border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">ID</th>
          <th class="border border-gray-300 px-4 py-2">Username</th>
          <th class="border border-gray-300 px-4 py-2">Email</th>
          <th class="border border-gray-300 px-4 py-2">Role</th>
          <th class="border border-gray-300 px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-4 py-2 text-center">{{ user.id }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ user.username }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ user.email }}</td>
          <td class="border border-gray-300 px-4 py-2 capitalize text-center">{{ user.role }}</td>
          <td class="border border-gray-300 px-4 py-2 space-x-2 text-center">
            <button @click="openModal(user)" class="text-blue-600 hover:underline">
              <Icon icon="ic:outline-edit-calendar" width="23" height="23"  style="color: #5180e9" />
            </button>
            <button @click="deleteUser(user.id!)" class="text-red-600 hover:underline">
              <Icon icon="ic:baseline-delete-forever" width="24" height="24" style="color: #f52222" />
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modal -->
    <div
      v-if="showModal"
      class="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center"
    >
      <div class="bg-white rounded p-6 w-96">
        <h2 class="text-xl font-semibold mb-4">{{ editingUser ? 'Edit User' : 'Add User' }}</h2>

        <form @submit.prevent="submitForm">
          <label class="block mb-2">
            Username:
            <input v-model="form.username" type="text" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            Email:
            <input v-model="form.email" type="email" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2" v-if="!editingUser">
            Password:
            <input v-model="form.password" type="password" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            Role:
            <select v-model="form.role" required class="w-full border rounded px-2 py-1">
              <option value="">Select Role</option>
              <option value="Admin">Admin</option>
              <option value="User">Staff</option>
            </select>
          </label>

          <div class="mt-4 flex justify-end space-x-2">
            <button type="button" @click="closeModal" class="px-4 py-2 rounded border">Cancel</button>
            <button type="submit" class="px-4 py-2 bg-purple-600 text-white rounded hover:bg-purple-700">
              {{ editingUser ? 'Update' : 'Create' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, ref } from 'vue'
import type { CreateDto, User } from '../Types/UserInterface'
import { createUser, deleteUserById, getUser, updateUser, } from '../Services/UserService'

const users = ref<User[]>([])

const showModal = ref(false)
const editingUser = ref<User | null>(null)

const form = ref<CreateDto>({
  username: '',
  email: '',
  password: '',
  role: '',
})


const fetchUsers = async () => {
  try {
    users.value = await getUser() 
  } catch (error) {
    // error handling
  }
}

const openModal = (user: User | null = null) => {
  if (user) {
    editingUser.value = user
    // Exclude password from edit form
    form.value = {
      username: user.username,
      email: user.email,
      password: '',
      role: user.role,
    }
  } else {
    editingUser.value = null
    form.value = {
      username: '',
      email: '',
      password: '',
      role: '',
    }
  }
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
}

const submitForm = async () => {
  try {
    if (editingUser.value) {
      await updateUser(editingUser.value.id!, form.value)
    } else {
      await createUser(form.value)
    }
    await fetchUsers()
    closeModal()
  } catch (error) {
    alert('Error saving user')
  }
}

const deleteUser = async (id: number) => {
  if (!confirm('Are you sure you want to delete this user?')) return
  try {
    await deleteUserById(id)  
    users.value = users.value.filter(u => u.id !== id)
  } catch (error) {
    alert('Error deleting user')
  }
}

onMounted(async () => {
  await fetchUsers()
})
</script>
