<template>
<div class="container">
    <RouterLink to="/admin/create-account" class="button">Create account</RouterLink>
    <div class="columns">
        <div class="column is-2">
            <p class="title">
                Admins
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
                Companies
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
                Employees
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
</div>
</template>

<script setup lang="ts">
import AdminAccountModel from '@/model/admin-account/AdminAccountModel';
import { onMounted, ref, Ref } from 'vue' 
import EmployeeAccountModel from '@/model/employee-account/EmployeeAccountModel';
import CompanyAccountModel from '@/model/company-account/CompanyAccountModel';
import axios from 'axios';
import useAuthStore, { AuthType } from '@/stores/AuthStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

if(authStore.currentAuthType != AuthType.Admin) {
    router.push('/admin/login')
}

const admins: Ref<AdminAccountModel[]> = ref([])
const employees: Ref<EmployeeAccountModel[]> = ref([])
const companies: Ref<CompanyAccountModel[]> = ref([])

onMounted(() => {
    axios.get<AdminAccountModel[]>('admin-account').then(res => admins.value = res.data)
    axios.get<CompanyAccountModel[]>('company-account').then(res => companies.value = res.data)
    axios.get<EmployeeAccountModel[]>('employee-account').then(res => employees.value = res.data)
})

</script>