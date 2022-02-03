import { createWebHistory, createRouter } from "vue-router";

import Home from './pages/Home.vue';

const routes = [
	{
		path: '/',
		name: 'Home',
		component: Home
	},
	{
		path: '/Contributors',
		name: 'Contributors',
		component: () => import('./pages/Contributors.vue')
	},
	{
		path: '/Address',
		name: 'Address',
		component: () => import('./pages/Address.vue')
	},
];

const router = createRouter({
	history: createWebHistory(),
	routes
});

export default router;