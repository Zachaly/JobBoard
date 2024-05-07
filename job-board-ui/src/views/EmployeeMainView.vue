<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-4">
                <button class="button is-info" @click="goToCreateAccount">Create account</button>
            </div>
        </div>
    </ViewTemplate>    
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import ViewTemplate from './ViewTemplate.vue';

const router = useRouter()

const goToCreateAccount = () => {
    router.push('/employee/create-account')
}
</script>