import useAuthStore, { AuthType } from "@/stores/AuthStore";
import AdminLoginView from "@/views/AdminLoginView.vue";
import AdminMainView from "@/views/AdminMainView.vue";
import CompanyMainViewVue from "@/views/CompanyMainView.vue";
import CreateAdminAccountView from "@/views/CreateAdminAccountView.vue";
import CreateCompanyAccountViewVue from "@/views/CreateCompanyAccountView.vue";
import CreateEmployeeAccountViewVue from "@/views/CreateEmployeeAccountView.vue";
import EmployeeMainViewVue from "@/views/EmployeeMainView.vue";
import CompanyLoginView from "@/views/CompanyLoginView.vue";
import EmployeeLoginView from "@/views/EmployeeLoginView.vue";
import CompanyProfileView from "@/views/CompanyProfileView.vue";
import EmployeeProfileView from "@/views/EmployeeProfileView.vue";
import UpdateCompanyAccountView from "@/views/UpdateCompanyAccountView.vue";
import UpdateEmployeeAccountView from "@/views/UpdateEmployeeAccountView.vue";
import AddJobOfferView from "@/views/AddJobOfferView.vue";
import SearchJobOffersView from "@/views/SearchJobOffersView.vue";
import JobOfferView from "@/views/JobOfferView.vue";
import CompanyProfileJobOffersView from "@/views/CompanyProfileJobOffersView.vue";
import UpdateJobOfferView from "@/views/UpdateJobOfferView.vue";
import ManageBusinessesView from "@/views/ManageBusinessesView.vue";
import { Component } from "vue";
import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";

const createRoute = (
  path: string,
  name: string,
  component: Component,
  authType?: AuthType
) => ({ path, name, component, meta: { authType } });

const routes: Array<RouteRecordRaw> = [
  { path: "/", redirect: "/employee" },
  createRoute("/company", "company-main", CompanyMainViewVue),
  createRoute(
    "/company/create-account",
    "company-create-account",
    CreateCompanyAccountViewVue
  ),
  createRoute("/employee", "employee-main", EmployeeMainViewVue),
  createRoute(
    "/employee/create-account",
    "employee-create-account",
    CreateEmployeeAccountViewVue
  ),
  createRoute("/admin", "admin-panel", AdminMainView, AuthType.Admin),
  createRoute(
    "/admin/create-account",
    "admin-create-account",
    CreateAdminAccountView,
    AuthType.Admin
  ),
  createRoute("/admin/login", "admin-login", AdminLoginView),
  createRoute("/company/login", "company-login", CompanyLoginView),
  createRoute("/employee/login", "employee-login", EmployeeLoginView),
  createRoute(
    "/company/profile",
    "company-profile",
    CompanyProfileView,
    AuthType.Company
  ),
  createRoute(
    "/employee/profile",
    "employee-profile",
    EmployeeProfileView,
    AuthType.Employee
  ),
  createRoute(
    "/company/profile/update",
    "company-profile-update",
    UpdateCompanyAccountView,
    AuthType.Company
  ),
  createRoute(
    "/employee/profile/update",
    "employee-update-profile",
    UpdateEmployeeAccountView,
    AuthType.Employee
  ),
  createRoute(
    "/job-offer/add",
    "add-job-offer",
    AddJobOfferView,
    AuthType.Company
  ),
  createRoute("/job-offer/search", "search-job-offers", SearchJobOffersView),
  createRoute("/job-offer/:id", "job-offer", JobOfferView),
  createRoute(
    "/company/profile/offers",
    "company-profile-job-offers",
    CompanyProfileJobOffersView,
    AuthType.Company
  ),
  createRoute(
    "/company/update-offer/:id",
    "update-job-offer",
    UpdateJobOfferView,
    AuthType.Company
  ),
  createRoute(
    "/admin/businesses",
    "manage-businesses",
    ManageBusinessesView,
    AuthType.Admin
  ),
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore();

  if (authStore.currentAuthType == AuthType.NotAuthorized) {
    await authStore.loadSavedUser();
  }

  if (
    to.meta.authType == AuthType.Admin &&
    authStore.currentAuthType != AuthType.Admin
  ) {
    next({ path: "/admin/login" });
    return;
  }

  if (
    to.meta.authType == AuthType.Company &&
    authStore.currentAuthType != AuthType.Company
  ) {
    next({ path: "/company/login" });
    return;
  }

  if (
    to.meta.authType == AuthType.Employee &&
    authStore.currentAuthType != AuthType.Employee
  ) {
    next({ path: "/employee/login" });
    return;
  }

  next();
});

export default router;
