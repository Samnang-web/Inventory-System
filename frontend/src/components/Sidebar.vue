<template>
  <div class="h-screen w-64 bg-gray-800 text-white flex flex-col">
    <div class="p-4 text-2xl font-bold border-b border-gray-700">
      Inventory
    </div>

    <nav class="flex-1 p-4 space-y-2">
      <RouterLink
        v-for="item in filteredMenu"
        :key="item.name"
        :to="item.path"
        class="block px-3 py-2 rounded hover:bg-gray-700 transition"
        active-class="bg-gray-700"
      >
        {{ item.name }}
      </RouterLink>
    </nav>

    <div class="p-4 border-t border-gray-700">
      <button
        @click="logout"
        class="w-full text-left px-3 py-2 rounded hover:bg-red-600 transition"
      >
        Logout
      </button>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const userRole = ref<string>('')

const allMenu = [
  { name: 'Dashboard', path: '/dashboard' },
  { name: 'Products', path: '/dashboard/products' },
  { name: 'Categories', path: '/dashboard/categories' },
  { name: 'Stock Transactions', path: '/dashboard/stocks' },
  { name: 'Users', path: '/dashboard/users', requiresAdmin: true },
]

const filteredMenu = computed(() =>
  allMenu.filter(
    item => !item.requiresAdmin || userRole.value.toLowerCase() === 'admin'
  )
)

const logout = () => {
  localStorage.removeItem('Token')
  localStorage.removeItem('role')
  router.push('/')
}

onMounted(() => {
  userRole.value = localStorage.getItem('role') || ''
})
</script>
