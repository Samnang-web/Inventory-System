<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white shadow-lg rounded-xl p-8 w-full max-w-md">
      <h2 class="text-2xl font-bold text-center text-gray-800 mb-6">
        Login to Your Account
      </h2>
      <form @submit.prevent="handleLogin">
        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="email">Email</label>
          <input
            v-model="email"
            type="email"
            id="email"
            placeholder="you@example.com"
            class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring focus:border-blue-400"
            required
          />
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="password"
            >Password</label
          >
          <input
            v-model="password"
            type="password"
            id="password"
            placeholder="••••••••"
            class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring focus:border-blue-400"
            required
          />
        </div>
        <div class="mb-4 text-red-500 text-sm" v-if="error">{{ error }}</div>
        <button
          type="submit"
          class="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded-lg transition"
        >
          Login
        </button>
      </form>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { Login } from "../../Services/UserService";

const router = useRouter();
const error = ref<string | null>(null);

const email = ref("");
const password = ref("");

const handleLogin = async () => {
  error.value = null;
  try {
    const user = await Login({ email: email.value, password: password.value });
    localStorage.setItem("token", user.token!);
    localStorage.setItem("role", user.role);
    localStorage.setItem("username", user.username);
    router.push("/dashboard");
  } catch (err: any) {
    error.value = err.response?.data?.message || "Login failed";
  }
};
</script>
