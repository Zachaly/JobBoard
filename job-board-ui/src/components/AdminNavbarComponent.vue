<template>
    <div class="navbar-menu container">
        <div class="navbar-start">
            <div class="navbar-item">
                <RouterLink to="/admin" class="button">Main</RouterLink>
                <RouterLink to="/admin/businesses" class="button">Manage businesses</RouterLink>
            </div>
        </div>
        <div class="navbar-end">
            <div class="navbar-item">
                <dib class="buttons">
                    <RouterLink to="/admin/create-account" class="button">Create account</RouterLink>
                    <button class="button is-danger" @click="logout">Logout</button>
                </dib>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import useAuthStore from '@/stores/AuthStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
    authStore.logout()
    router.push('/company/login')
}
</script>