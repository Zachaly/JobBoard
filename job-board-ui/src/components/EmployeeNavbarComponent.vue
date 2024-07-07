<template>
    <nav class="navbar-menu container">
        <div class="navbar-start">
            <router-link class="navbar-item" active-class="is-active" to="/job-offer/search">Search offers</router-link>
        </div>
        <div class="navbar-end" v-if="authStore.currentAuthType != AuthType.Employee">
            <div class="navbar-item">
                <div class="buttons">
                    <router-link to="/employee/login" class="button is-primary">Login</router-link>
                    <router-link to="/employee/create-account" class="button">Create account</router-link>
                </div>
            </div>
        </div>
        <div class="navbar-end" v-else>
            <div class="navbar-item">
                Hello {{ authStore.employeeData?.firstName }}
            </div>
            <div class="navbar-item">
                <div class="buttons">
                    <router-link class="button" to="/employee/profile">Profile</router-link>
                    <button class="button is-danger" @click="logout()">Logout</button>
                </div>
            </div>
        </div>
    </nav>
</template>

<script setup lang="ts">
import useAuthStore, { AuthType } from '@/stores/AuthStore';
import { useRouter } from 'vue-router';
const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
    authStore.logout()
    router.push('/employee/login')
}
</script>