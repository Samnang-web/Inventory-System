<template>
  <div class="p-6 bg-white rounded shadow max-w-4xl mx-auto mt-10">
    <h1 class="text-3xl font-bold mb-6">Categories</h1>

    <button
      @click="openModal()"
      class="mb-6 px-5 py-2 bg-indigo-600 text-white rounded hover:bg-indigo-700"
    >
      Add Category
    </button>

    <table class="min-w-full table-auto border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">ID</th>
          <th class="border border-gray-300 px-4 py-2">Name</th>
          <th class="border border-gray-300 px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="category in categories" :key="category.id" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-4 py-2 text-center">{{ category.id }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ category.name }}</td>
          <td class="border border-gray-300 px-4 py-2 space-x-2 text-center">
            <button @click="openModal(category)" class="text-blue-600 hover:underline">
              <Icon icon="ic:outline-edit-calendar" width="23" height="23"  style="color: #5180e9" />
            </button>
            <button @click="deleteCategory(category.id!)" class="text-red-600 hover:underline">
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
        <h2 class="text-xl font-semibold mb-4">
          {{ editingCategory ? 'Edit Category' : 'Add Category' }}
        </h2>

        <form @submit.prevent="submitForm">
          <label class="block mb-2">
            Name:
            <input
              v-model="form.name"
              type="text"
              required
              class="w-full border rounded px-2 py-1"
            />
          </label>

          

          <div class="mt-4 flex justify-end space-x-2">
            <button
              type="button"
              @click="closeModal"
              class="px-4 py-2 rounded border"
            >
              Cancel
            </button>
            <button
              type="submit"
              class="px-4 py-2 bg-indigo-600 text-white rounded hover:bg-indigo-700"
            >
              {{ editingCategory ? 'Update' : 'Create' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, onMounted, ref } from 'vue'
import type { Category } from '../Types/CategoryInterface'
import { useCategoryStore } from '../Store/CategoriesStore'

const store = useCategoryStore()
const categories = computed(() => store.category)

const showModal = ref(false)
const editingCategory = ref<Category | null>(null)


const form = ref<Category>({
  id: 0,
  name: '',
})

const openModal = (category: Category | null = null) => {
  if (category) {
    editingCategory.value = category
    form.value = { ...category }
  } else {
    editingCategory.value = null
    form.value = {
      id: 0,
      name: '',
    }
  }
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
}

const submitForm = async () => {
  try{
    if(editingCategory.value){
      await store.editCategories(editingCategory.value.id, form.value);
    }else{
      await store.createCategories(form.value)
    }
    await store.fetchCategories()
    closeModal();

  }catch(error){

    alert('Failed Creat and Update Category!')
  }
}

const deleteCategory = async (id: number) => {
  if (!id) return
  const confirmDelete = confirm('Are you sure you want to delete this Category?')
  if (!confirmDelete) return

  try {
    await store.removeCategories(id)
    alert('Category deleted successfully!')
  } catch (error) {
    console.error('Failed to delete Category:', error)
    alert('Failed to delete Category, please try again.')
  }
}

onMounted(store.fetchCategories);
</script>
