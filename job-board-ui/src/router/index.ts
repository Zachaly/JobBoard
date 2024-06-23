import AdminMainView from '@/views/AdminMainView.vue'
import CompanyMainViewVue from '@/views/CompanyMainView.vue'
import CreateAdminAccountView from '@/views/CreateAdminAccountView.vue'
import CreateCompanyAccountViewVue from '@/views/CreateCompanyAccountView.vue'
import CreateEmployeeAccountViewVue from '@/views/CreateEmployeeAccountView.vue'
import EmployeeMainViewVue from '@/views/EmployeeMainView.vue'
import { Component } from 'vue'
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const createRoute = (path: string, name: string, component: Component) => ({ path, name, component })

const routes: Array<RouteRecordRaw> = [
  { path: '/', redirect: '/company'},
  createRoute('/company', 'company-main', CompanyMainViewVue),
  createRoute('/company/create-account', 'company-create-account', CreateCompanyAccountViewVue),
  createRoute('/employee', 'employee-main', EmployeeMainViewVue),
  createRoute('/employee/create-account', 'employee-create-account', CreateEmployeeAccountViewVue),
  createRoute('/admin', 'admin-panel', AdminMainView),
  createRoute('/admin/create-account', 'admin-create-account', CreateAdminAccountView)
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
