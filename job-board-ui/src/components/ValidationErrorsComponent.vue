<template>
    <div v-if="errors != null">
        <p class="has-text-danger"  v-for="error of errors" :key="error">
            {{ error }}
        </p>
    </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';

defineProps<{
    errors?: string[]
}>()
</script>