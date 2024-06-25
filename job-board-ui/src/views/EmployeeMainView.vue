<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-4">
                <button class="button is-info" @click="goToCreateAccount">Create account</button>
                <RouterLink to="/employee/login" class="button">Login</RouterLink>
            </div>
        </div>
    </ViewTemplate>    
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router';
import ViewTemplate from './ViewTemplate.vue';

const router = useRouter()

const goToCreateAccount = () => {
    router.push('/employee/create-account')
}
</script>