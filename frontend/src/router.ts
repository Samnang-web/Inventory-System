import { createRouter, createWebHistory } from 'vue-router'

import Dashboard from './Pages/Dashboard.vue'
import Layout from './components/Layout.vue'
import Products from './Pages/Products.vue'
import Login from './Pages/Login/login.vue'
import Categories from './Pages/Categories.vue'
import Stock from './Pages/Stock.vue'
import User from './Pages/User.vue'


const routes = [
  {
    path: '/',
    name: 'Login',
    component: Login
  },
  {
    path: '/dashboard',
    component: Layout,
    children: [
      {
        path: '',
        name: 'Dashboard',
        component: Dashboard,
      },
      {
        path: 'products',
        name: 'Products',
        component: Products,
      },
      {
        path: 'categories',
        name: 'Categories',
        component: Categories
      },
      {
        path: 'stocks',
        name: 'Stock',
        component: Stock
      },
      {
        path: 'users',
        name: 'User',
        component: User,
        meta: {requiresAuth:true, requiredRole: 'Admin'}
      },
      
      
    ],
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, _, next) => {
  const token = localStorage.getItem('token')
  const role = localStorage.getItem('role')?.toLowerCase()

  if (to.meta.requiresAuth) {
    if (!token) {
      return next({ name: 'Login' })
    }

    const requiredRole = to.meta.requiredRole
    if (
  typeof requiredRole === 'string' &&
  requiredRole.toLowerCase() !== role?.toLowerCase()
) {
  alert('Access Denied: Admins only')
  return next('/dashboard')
}

    return next()
  }

  return next() 
})

export default router
