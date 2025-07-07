<template>
  <div class="p-6 bg-white rounded shadow">
    <h2 class="text-xl font-bold mb-4">📊 Stock Summary (Current Stock)</h2>

    <table class="w-full table-auto border-collapse border border-gray-300">
      <thead class="bg-gray-100">
        <tr>
          <th class="border px-4 py-2">Product ID</th>
          <th class="border px-4 py-2">Total IN</th>
          <th class="border px-4 py-2">Total OUT</th>
          <th class="border px-4 py-2">Current Stock</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in stockSummary" :key="item.product_id" class="hover:bg-gray-50">
          <td class="border px-4 py-2">{{ item.product_id }}</td>
          <td class="border px-4 py-2">{{ item.totalIn }}</td>
          <td class="border px-4 py-2">{{ item.totalOut }}</td>
          <td class="border px-4 py-2 font-semibold">{{ item.current }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, ref, computed } from 'vue'
import { getStock } from '../Services/StockService'
import type { Stock } from '../Types/StockInterface'

const stocks = ref<Stock[]>([])

onMounted(async () => {
  stocks.value = await getStock()
})

const stockSummary = computed(() => {
  const result: Record<number, { product_id: number; totalIn: number; totalOut: number }> = {}

  for (const stock of stocks.value) {
    const pid = stock.product_Id
    if (!result[pid]) {
      result[pid] = { product_id: pid, totalIn: 0, totalOut: 0 }
    }
    if (stock.types === 'IN') result[pid].totalIn += stock.quantity
    if (stock.types === 'OUT') result[pid].totalOut += stock.quantity
  }

  return Object.values(result).map(item => ({
    ...item,
    current: item.totalIn - item.totalOut,
  }))
})
</script>
