<template>
  <div class="p-6 md:p-10 max-w-7xl mx-auto space-y-10">
    <h1 class="text-4xl font-bold text-gray-800">Dashboard Overview</h1>

    <p class="text-sm text-gray-600">
      Logged in as: <strong>{{ currentUser?.username || 'Unknown' }}</strong>
    </p>

    <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6">
      <div class="bg-blue-500 text-white rounded-2xl p-6 shadow hover:shadow-lg transition">
        <h2 class="text-lg font-medium">Total Products</h2>
        <p class="text-4xl font-semibold mt-2">{{ totalProducts }}</p>
      </div>

      <div class="bg-green-500 text-white rounded-2xl p-6 shadow hover:shadow-lg transition">
        <h2 class="text-lg font-medium">Total Users</h2>
        <p class="text-4xl font-semibold mt-2">{{ totalUsers }}</p>
      </div>

      <div class="bg-purple-500 text-white rounded-2xl p-6 shadow hover:shadow-lg transition">
        <h2 class="text-lg font-medium">Stock Transactions</h2>
        <p class="text-4xl font-semibold mt-2">{{ totalStocks }}</p>
      </div>
    </div>

    <div>
      <h2 class="text-2xl font-semibold text-gray-800 mb-4">Recent Products</h2>
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        <div
          v-for="product in recentProducts"
          :key="product.id"
          class="border border-gray-200 rounded-lg p-4 shadow-sm hover:shadow-md transition"
        >
          <h3 class="text-lg font-semibold text-gray-700">{{ product.name }}</h3>
          <p class="text-sm text-gray-500">SKU: {{ product.sku }}</p>
          <p class="text-sm text-gray-600 mt-2">Qty: {{ product.quantity }}</p>
          <p class="text-sm text-gray-600">Price: ${{ product.unitPrice.toFixed(2) }}</p>
        </div>
      </div>
    </div>
  </div>
</template>


<script lang="ts" setup>
import { computed } from 'vue'
import { useProductStore } from '../Store/ProductStore'
import { useUserStore } from '../Store/UserStore';
import { useStockStore } from '../Store/StockStore';

const storeProduct = useProductStore();
const storeUser = useUserStore();
const storeStock = useStockStore();

const totalProducts = computed(() => storeProduct.products.length)
const totalUsers = computed(() => storeUser.users.length)
const totalStocks = computed(() => storeStock.stock.length)
const currentUser = computed(() => storeUser.currentUser)


const recentProducts = computed(() => {
  return [...storeProduct.products].slice(-5).reverse()
})
</script>
