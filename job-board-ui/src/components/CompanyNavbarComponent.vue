<template>
    <nav class="navbar-menu container">
        <div class="nav-bar-start">
            <div class="navbar-item">
                <router-link class="button" to="/company">Main</router-link>
            </div>
        </div>
        <div class="navbar-end" v-if="authStore.currentAuthType != AuthType.Company">
            <div class="navbar-item">
                <div class="buttons">
                    <router-link to="/company/login" class="button is-primary">Login</router-link>
                    <router-link to="/company/create-account" class="button">Create account</router-link>
                </div>
            </div>
        </div>
        <div class="navbar-end" v-else>
            <div class="navbar-item">
                Hello {{ authStore.companyData?.name}}
            </div>
            <div class="navbar-item">
                <div class="buttons">
                    <router-link to="/company/profile" class="button">Profile</router-link>
                    <router-link to="/job-offer/add" class="button">Add job offer</router-link>
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
    router.push('/company/login')
}
</script>