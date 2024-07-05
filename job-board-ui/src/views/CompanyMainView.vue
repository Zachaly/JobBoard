<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-4 is-centered">
                <button @click="goToCreateAccount" class="button is-info">
                    Create account
                </button>
                <RouterLink to="/company/login" class="button">Login</RouterLink>
                <RouterLink to="/company/profile" class="button">Profile</RouterLink>
                <RouterLink to="/job-offer/add" class="button">Add job offer</RouterLink>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router';
import ViewTemplate from './ViewTemplate.vue';

const router = useRouter()

const goToCreateAccount = () => {
    router.push('/company/create-account')
}
</script>