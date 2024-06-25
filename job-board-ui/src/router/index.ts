import useAuthStore, { AuthType } from '@/stores/AuthStore'
import AdminLoginView from '@/views/AdminLoginView.vue'
import AdminMainView from '@/views/AdminMainView.vue'
import CompanyMainViewVue from '@/views/CompanyMainView.vue'
import CreateAdminAccountView from '@/views/CreateAdminAccountView.vue'
import CreateCompanyAccountViewVue from '@/views/CreateCompanyAccountView.vue'
import CreateEmployeeAccountViewVue from '@/views/CreateEmployeeAccountView.vue'
import EmployeeMainViewVue from '@/views/EmployeeMainView.vue'
import CompanyLoginView from '@/views/CompanyLoginView.vue'
import EmployeeLoginView from '@/views/EmployeeLoginView.vue'
import { Component } from 'vue'
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const createRoute = (path: string, name: string, component: Component, authType?: AuthType) => ({ path, name, component, meta: { authType } })

const routes: Array<RouteRecordRaw> = [
  { path: '/', redirect: '/company'},
  createRoute('/company', 'company-main', CompanyMainViewVue),
  createRoute('/company/create-account', 'company-create-account', CreateCompanyAccountViewVue),
  createRoute('/employee', 'employee-main', EmployeeMainViewVue),
  createRoute('/employee/create-account', 'employee-create-account', CreateEmployeeAccountViewVue),
  createRoute('/admin', 'admin-panel', AdminMainView, AuthType.Admin),
  createRoute('/admin/create-account', 'admin-create-account', CreateAdminAccountView, AuthType.Admin),
  createRoute('/admin/login', 'admin-login', AdminLoginView),
  createRoute('/company/login', 'company-login', CompanyLoginView),
  createRoute('/employee/login', 'employee-login', EmployeeLoginView)
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  if(to.meta.authType == AuthType.Admin && authStore.currentAuthType != AuthType.Admin) {
    next({ name: 'admin-login' })
    return
  }

  if(to.meta.authType == AuthType.Company && authStore.currentAuthType != AuthType.Company) {
    next({ name: 'company-login' })
    return
  }

  if(to.meta.authType == AuthType.Employee && authStore.currentAuthType != AuthType.Employee) {
    next({ name: 'employee-login'})
    return
  }

  next()
})

export default router
