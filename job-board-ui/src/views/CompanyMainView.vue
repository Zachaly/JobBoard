<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-4 is-centered">
                <button @click="goToCreateAccount" class="button is-info">
                    Create account
                </button>
                <RouterLink to="/company/login" class="button">Login</RouterLink>
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