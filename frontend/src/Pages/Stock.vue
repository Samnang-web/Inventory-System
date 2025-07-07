<template>
  <div class="h-screen flex">
    <!-- Main Scrollable Content -->
    <div class="flex-1 overflow-y-auto p-6 bg-white rounded shadow">
      <h1 class="text-2xl font-bold mb-4">Stock Transactions</h1>

      <!-- Toolbar -->
      <div class="flex flex-wrap items-center justify-between mb-4 gap-2">
        <div class="flex items-center gap-2">
          <input
            v-model="searchQuery"
            placeholder="Search by note or type"
            class="border rounded px-3 py-1"
          />
          <select v-model="filterType" class="border rounded px-3 py-1">
            <option value="">All Types</option>
            <option value="IN">IN</option>
            <option value="OUT">OUT</option>
          </select>
          <input
            v-model="filterUser"
            type="number"
            placeholder="Created By ID"
            class="border rounded px-3 py-1 w-32"
          />
        </div>

        <button
          @click="openModal()"
          class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          Add Stock
        </button>
      </div>

      <!-- Table -->
      <table class="min-w-full table-auto border-collapse border border-gray-200">
        <thead>
          <tr class="bg-gray-100 text-sm">
            <th class="border px-3 py-2">ID</th>
            <th class="border px-3 py-2">Product ID</th>
            <th class="border px-3 py-2">Quantity</th>
            <th class="border px-3 py-2">Type</th>
            <th class="border px-3 py-2">Note</th>
            <th class="border px-3 py-2">Created By</th>
            <th class="border px-3 py-2">Date</th>
            <th class="border px-3 py-2">Actions</th>
          </tr>
        </thead>
        <tbody>
        <tr
          v-for="stock in paginatedStocks"
          :key="stock.id"
          class="hover:bg-gray-50 text-sm"
        >
          <td class="border px-3 py-2 text-center align-middle">{{ stock.id }}</td>
          <td class="border px-3 py-2 text-center align-middle">
            {{ products.find(prod => prod.id === stock.product_Id)?.name || 'N/A' }}
          </td>
          <td class="border px-3 py-2 text-center align-middle">{{ stock.quantity }}</td>
          <td class="border px-3 py-2 text-center align-middle">{{ stock.types }}</td>
          <td class="border px-3 py-2 text-center align-middle">{{ stock.note }}</td>
          <td class="border px-3 py-2 text-center align-middle">
            {{ users.find(user => user.id === stock.created_by)?.username || 'N/A' }}
          </td>
          <td class="border px-3 py-2 text-center align-middle">
            {{ formatDate(stock.createdAt) }}
          </td>
          <td class="border px-3 py-2 text-center align-middle">
            <div class="flex justify-center items-center space-x-3">
              <button @click="openModal(stock)">
                <Icon icon="ic:outline-edit-calendar" width="23" height="23"  style="color: #5180e9" />
              </button>
              <button @click="deleteStocks(stock.id)">
                <Icon icon="ic:baseline-delete-forever" width="24" height="24" style="color: #f52222" />
              </button>
            </div>
          </td>
        </tr>
        <tr v-if="paginatedStocks.length === 0">
          <td colspan="8" class="text-center py-4 text-gray-500">
            No stock records found.
          </td>
        </tr>
      </tbody>
      </table>

      <!-- Pagination -->
      <div class="mt-4 flex justify-between items-center">
        <span>
          Showing {{ startIndex + 1 }}–{{ Math.min(endIndex, filteredStocks.length) }}
          of {{ filteredStocks.length }}
        </span>
        <div class="flex gap-2">
          <button
            :disabled="currentPage === 1"
            @click="currentPage--"
            class="px-2 py-1 border rounded hover:bg-gray-100"
          >
            « Prev
          </button>
          <button
            :disabled="endIndex >= filteredStocks.length"
            @click="currentPage++"
            class="px-2 py-1 border rounded hover:bg-gray-100"
          >
            Next »
          </button>
        </div>
      </div>

      <!-- Modal -->
      <div
        v-if="showModal"
        class="fixed inset-0 bg-black bg-opacity-30 z-50 flex items-center justify-center px-4"
      >
        <div
          class="bg-white w-full max-w-md rounded-lg shadow-lg overflow-y-auto max-h-[90vh] relative"
        >
          <!-- Close Button -->
          <button
            @click="closeModal"
            class="absolute top-2 right-2 text-gray-400 hover:text-red-500"
            aria-label="Close"
          >
            ✕
          </button>

          <div class="p-6">
            <h2 class="text-xl font-semibold text-gray-800 mb-4">
              {{ editingStock ? "Edit Stock" : "Add Stock" }}
            </h2>

            <form @submit.prevent="submitForm" class="space-y-4">
              <!-- Product ID -->
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Product</label>
                <select
                  v-model="form.product_Id"
                  required
                  class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="" disabled>Select a product</option>
                  <option
                    v-for="product in products"
                    :key="product.id"
                    :value="product.id"
                  >
                    {{ product.name }}
                  </option>
                </select>
              </div>

              <!-- Quantity -->
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Quantity</label>
                <input
                  v-model="form.quantity"
                  type="number"
                  required
                  class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>

              <!-- Type -->
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Type</label>
                <select
                  v-model="form.types"
                  required
                  class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="" disabled>Select Type</option>
                  <option value="IN">IN</option>
                  <option value="OUT">OUT</option>
                </select>
              </div>

              <!-- Note -->
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Note</label>
                <input
                  v-model="form.note"
                  type="text"
                  placeholder="Optional note"
                  class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>

              
              <!-- Actions -->
              <div class="pt-4 flex justify-end gap-2">
                <button
                  type="button"
                  @click="closeModal"
                  class="px-4 py-2 border rounded hover:bg-gray-100"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
                >
                  {{ editingStock ? "Update" : "Create" }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import type { Stock, StockFormDto } from "../Types/StockInterface";
import { useStockStore } from "../Store/StockStore";
import { useProductStore } from "../Store/ProductStore";
import { useUserStore } from "../Store/UserStore";

const store = useStockStore();
const storeProduct = useProductStore();
const storeUser = useUserStore();

const stocks = computed(() => store.stock);
const products = computed(() => storeProduct.products);
const users = computed(() => storeUser.users);

const showModal = ref(false);
const editingStock = ref<Stock | null>(null);
const currentPage = ref(1);
const pageSize = 10;

const searchQuery = ref("");
const filterType = ref("");
const filterUser = ref("");

const form = ref<StockFormDto>({
  product_Id: 0,
  quantity: 0,
  types: "",
  note: "",
  created_by: 0,
});

const openModal = (stock: Stock | null = null) => {
  editingStock.value = stock;
  form.value = stock
    ? { ...stock }
    : { product_Id: 0, quantity: 0, types: "", note: "", created_by: 0 };
  showModal.value = true;
};

const closeModal = () => {
  showModal.value = false;
};

const submitForm = async () => {
  try {
    form.value.created_by = storeUser.currentUser?.id || 0;

    if (editingStock.value) {
      await store.editStock(editingStock.value.id, form.value);
    } else {
      await store.createStock(form.value);
    }

    await store.fetchStock()
    closeModal();
  } catch (error) {
    alert("Add/Update failed!");
  }
};

const deleteStocks = async (id?: number) => {
  if (!id) return;
  const confirmDelete = confirm("Are you sure you want to delete this product?");
  if (!confirmDelete) return;

  try {
    await store.removeStock(id);
    alert("Product deleted successfully!");
  } catch (error) {
    console.error("Failed to delete product:", error);
    alert("Failed to delete product, please try again.");
  }
};

const formatDate = (date: Date | string) => {
  return new Date(date).toLocaleString();
};

// Filtering
const filteredStocks = computed(() => {
  return stocks.value.filter((stock) => {
    const matchesSearch =
      stock.note.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      stock.types.toLowerCase().includes(searchQuery.value.toLowerCase());
    const matchesType = filterType.value ? stock.types === filterType.value : true;
    const matchesUser = filterUser.value ? stock.created_by === +filterUser.value : true;
    return matchesSearch && matchesType && matchesUser;
  });
});

// Pagination
const startIndex = computed(() => (currentPage.value - 1) * pageSize);
const endIndex = computed(() => startIndex.value + pageSize);
const paginatedStocks = computed(() =>
  filteredStocks.value.slice(startIndex.value, endIndex.value)
);

onMounted(async () => {
  await store.fetchStock();
  await storeProduct.fetchProduct();
  await storeUser.fetchUser();
});
</script>
