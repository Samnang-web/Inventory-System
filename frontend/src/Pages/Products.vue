<template>
  <div class="p-6 bg-white rounded shadow max-w-7xl mx-auto mt-10">
    <h1 class="text-3xl font-bold mb-6">Products</h1>
    <button
      @click="openModal()"
      class="mb-6 px-5 py-2 bg-green-600 text-white rounded hover:bg-green-700"
    >
      Add Product
    </button>

    <table class="min-w-full table-auto border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">ID</th>
          <th class="border border-gray-300 px-4 py-2">Image</th>
          <th class="border border-gray-300 px-4 py-2">Name</th>
          <th class="border border-gray-300 px-4 py-2">Description</th>
          <th class="border border-gray-300 px-4 py-2">SKU</th>
          <th class="border border-gray-300 px-4 py-2">Quantity</th>
          <th class="border border-gray-300 px-4 py-2">Unit Price</th>
          <th class="border border-gray-300 px-4 py-2">Category</th>
          <th class="border border-gray-300 px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in products" :key="product.id" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-4 py-2">{{ product.id }}</td>
          <td class="border border-gray-300 px-4 py-2">
            <img
              v-if="product.image_Url"
              :src="IMG_URL.replace('/api', '') + product.image_Url"
              alt="Product"
              class="w-16 h-16 object-cover rounded mx-auto"
            />
            <span v-else class="text-gray-400 italic">No image</span>
          </td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ product.name }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ product.description }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ product.sku }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ product.quantity }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">${{ product.unitPrice.toFixed(2) }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">
            {{ categories.find(cat => cat.id === product.category_Id)?.name || 'N/A' }}
          </td>
          <td class="border border-gray-300 px-4 py-2 space-x-2 text-center">
            <button @click="openModal(product)" class="text-blue-600 hover:underline">
              <Icon icon="ic:outline-edit-calendar" width="23" height="23"  style="color: #5180e9" />
            </button>
            <button @click="deleteProducts(product.id)" class="text-red-600 hover:underline">
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
        <h2 class="text-xl font-semibold mb-4">{{ editingProduct ? 'Edit Product' : 'Add Product' }}</h2>

        <form @submit.prevent="submitForm">
          <label class="block mb-2">
            Name:
            <input v-model="form.name" type="text" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            Description:
            <input v-model="form.description" type="text" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            SKU:
            <input v-model="form.sku" type="text" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            Quantity:
            <input v-model.number="form.quantity" type="number" min="0" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            Unit Price:
            <input v-model.number="form.unitPrice" type="number" min="0" step="0.01" required class="w-full border rounded px-2 py-1" />
          </label>

          <label class="block mb-2">
            Category:
            <select v-model.number="form.category_Id" required class="w-full border rounded px-2 py-1">
              <option disabled value="">-- Select Category --</option>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                {{ cat.name }}
              </option>
            </select>
          </label>

          <label class="block mb-2">
            Image:
            <input type="file" @change="handleImageUpload" />
            <div v-if="form.image_Url" class="mt-2">
              <img
                :src="typeof form.image_Url === 'string' && form.image_Url.startsWith('data:')
                  ? form.image_Url
                  : IMG_URL.replace('/api', '') + form.image_Url"
                alt="Preview"
                class="w-24 h-24 object-cover rounded"
              />
            </div>
          </label>

          <div class="mt-4 flex justify-end space-x-2">
            <button type="button" @click="closeModal" class="px-4 py-2 rounded border">Cancel</button>
            <button type="submit" class="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700">
              {{ editingProduct ? 'Update' : 'Create' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from 'vue'
import { useProductStore } from '../Store/ProductStore'
import type { Product } from '../Types/ProductInterface'
import { useCategoryStore } from '../Store/CategoriesStore'

const store = useProductStore()
const storeCategory = useCategoryStore()

const products = computed(() => store.products)
const categories = computed(() => storeCategory.category)

const showModal = ref(false)
const editingProduct = ref<Product | null>(null)
const IMG_URL = import.meta.env.VITE_API_URL

const form = ref<Partial<Product> & { imageFile?: File }>({
  name: '',
  sku: '',
  description: '',
  quantity: 0,
  unitPrice: 0,
  category_Id: 0,
})

onMounted(async () => {
  await store.fetchProduct()
  await storeCategory.fetchCategories()
})

const openModal = (product: Product | null = null) => {
  if (product) {
    editingProduct.value = product
    form.value = { ...product }
  } else {
    resetForm()
  }
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
}

const resetForm = () => {
  form.value = {
    name: '',
    sku: '',
    description: '',
    quantity: 0,
    unitPrice: 0,
    category_Id: 0,
    image_Url: '',
    imageFile: undefined,
  }
  editingProduct.value = null
}

const handleImageUpload = (e: Event) => {
  const file = (e.target as HTMLInputElement).files?.[0]
  if (file) {
    form.value.imageFile = file

    const reader = new FileReader()
    reader.onload = () => {
      form.value.image_Url = reader.result as string
    }
    reader.readAsDataURL(file)
  }
}

const buildFormData = () => {
  const fd = new FormData()
  fd.append('Name', form.value.name ?? '')
  fd.append('Sku', form.value.sku ?? '')
  fd.append('Description', form.value.description ?? '')
  fd.append('Quantity', form.value.quantity?.toString() ?? '0')
  fd.append('UnitPrice', form.value.unitPrice?.toString() ?? '0')
  fd.append('Category_Id', form.value.category_Id?.toString() ?? '0')
  if (form.value.imageFile) {
    fd.append('Image_Url', form.value.imageFile)
  }
  return fd
}

const submitForm = async () => {
  try {
    const fd = buildFormData()
    if (editingProduct.value) {
      await store.editProduct(editingProduct.value.id!, fd)
    } else {
      await store.createProduct(fd)
    }
    await store.fetchProduct()
    closeModal()
  } catch (err) {
    alert('Failed to save product!')
    console.error(err)
  }
}

const deleteProducts = async (id?: number) => {
  if (!id) return
  const confirmDelete = confirm('Are you sure you want to delete this product?')
  if (!confirmDelete) return

  try {
    await store.removeProduct(id)
    alert('Product deleted successfully!')
  } catch (error) {
    console.error('Failed to delete product:', error)
    alert('Failed to delete product, please try again.')
  }
}
</script>
