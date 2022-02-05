import { createWebHistory, createRouter } from 'vue-router'

import Home from './pages/Home.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/contributors',
    name: 'Contributors',
    component: () => import('./pages/Contributors.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
