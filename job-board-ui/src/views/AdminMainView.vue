<template>
    <ViewTemplate :admin-navbar="true">
        <div class="columns">
            <div class="column is-2">
                <p class="title">
                    Admins ({{ adminCount }})
                </p>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Login</th>
                        </tr>
                    </thead>

                    <tr v-for="admin of admins" :key="admin.id">
                        <td>
                            {{ admin.id }}
                        </td>
                        <td>
                            {{ admin.login }}
                        </td>
                    </tr>
                </table>
            </div>
            <div class="column">
                <p class="title">
                    Companies ({{ companyCount }})
                </p>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Name</th>
                            <th>Country</th>
                            <th>City</th>
                            <th>Contact email</th>
                        </tr>
                        <tr>
                            <td></td>
                            <td><input class="input" v-model="getCompaniesRequest.Name" @input="loadCompanies()" /></td>
                            <td><input class="input" v-model="getCompaniesRequest.Country" @input="loadCompanies()"> </td>
                            <td><input class="input" v-model="getCompaniesRequest.City" @input="loadCompanies()" /></td>
                            <td></td>
                        </tr>
                    </thead>
                    <tr v-for="company of companies" :key="company.id">
                        <td>
                            {{ company.id }}
                        </td>
                        <td>
                            {{ company.name }}
                        </td>
                        <td>
                            {{ company.country }}
                        </td>
                        <td>
                            {{ company.city }}
                        </td>
                        <td>
                            {{ company.contactEmail }}
                        </td>
                    </tr>
                </table>
            </div>
            <div class="column">
                <p class="title">
                    Employees ({{ employeeCount }})
                </p>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Country</th>
                            <th>City</th>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td><input class="input" v-model="getEmployeesRequest.Country" @input="loadEmployees()"></td>
                            <td><input class="input" v-model="getEmployeesRequest.City" @input="loadEmployees()"></td>
                        </tr>
                    </thead>
                    <tr v-for="employee of employees" :key="employee.id">
                        <td>
                            {{ employee.id }}
                        </td>
                        <td>
                            {{ employee.firstName }} {{ employee.lastName }}
                        </td>
                        <td>
                            {{ employee.email }}
                        </td>
                        <td>
                            {{ employee.country }}
                        </td>
                        <td>
                            {{ employee.city }}
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import AdminAccountModel from '@/model/admin-account/AdminAccountModel';
import { onMounted, ref, Ref } from 'vue'
import EmployeeAccountModel from '@/model/employee-account/EmployeeAccountModel';
import CompanyAccountModel from '@/model/company-account/CompanyAccountModel';
import axios from 'axios';
import useAuthStore, { AuthType } from '@/stores/AuthStore';
import { useRouter } from 'vue-router';
import GetCompanyAccountRequest from '../model/company-account/GetCompanyAccountRequest';
import GetEmployeeAccountRequest from '../model/employee-account/GetEmployeeAccountRequest';
import ViewTemplate from '@/views/ViewTemplate.vue';

const authStore = useAuthStore()
const router = useRouter()

if (authStore.currentAuthType != AuthType.Admin) {
    router.push('/admin/login')
}

const admins: Ref<AdminAccountModel[]> = ref([])
const adminCount = ref(0)
const employees: Ref<EmployeeAccountModel[]> = ref([])
const employeeCount = ref(0)
const companies: Ref<CompanyAccountModel[]> = ref([])
const companyCount = ref(0)

const getCompaniesRequest: Ref<GetCompanyAccountRequest> = ref({})
const getEmployeesRequest: Ref<GetEmployeeAccountRequest> = ref({})

onMounted(() => {
    axios.get<AdminAccountModel[]>('admin-account', { params: { SkipPagination: true }}).then(res => admins.value = res.data)
    axios.get<CompanyAccountModel[]>('company-account').then(res => companies.value = res.data)
    axios.get<EmployeeAccountModel[]>('employee-account').then(res => employees.value = res.data)

    axios.get<number>('admin-account/count').then(res => adminCount.value = res.data)
    axios.get<number>('company-account/count').then(res => companyCount.value = res.data)
    axios.get<number>('employee-account/count').then(res => employeeCount.value = res.data)
})

const loadEmployees = () => {
    axios.get<EmployeeAccountModel[]>('employee-account', { params: getEmployeesRequest.value })
        .then(res => employees.value = res.data)

    axios.get<number>('employee-account/count', { params: getEmployeesRequest.value })
        .then(res => employeeCount.value = res.data)
}

const loadCompanies = () => {
    axios.get<CompanyAccountModel[]>('company-account', { params: getCompaniesRequest.value })
        .then(res => companies.value = res.data)

    axios.get<number>('company-account/count', { params: getCompaniesRequest.value })
        .then(res => companyCount.value = res.data)
}
</script>