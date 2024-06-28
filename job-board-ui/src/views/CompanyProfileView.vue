<template>
    <div>
        <button class="button is-danger" @click="logout">Logout</button>
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