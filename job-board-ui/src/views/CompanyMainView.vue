<template>
    <div class="columns is-centered">
        <div class="column is-4 is-centered">
            <button @click="goToCreateAccount" class="button is-info">
                Create account
            </button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter()

const goToCreateAccount = () => {
    router.push('/company/create-account')
}

</script>