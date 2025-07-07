<template>
  <div class="flex h-screen overflow-hidden">
    <Sidebar />
    <div class="flex-1 bg-gray-100 min-h-screen p-6">
      <router-view />
    </div>
  </div>
</template>

<script lang="ts" setup>
 import Sidebar from './Sidebar.vue';
</script>
