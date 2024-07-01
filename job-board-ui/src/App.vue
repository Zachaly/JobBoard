<template>
  <router-view :key="$route.path" />
</template>

<script setup lang="ts">
import useAuthStore from './stores/AuthStore';
import { onMounted } from 'vue';

const authStore = useAuthStore()

onMounted(async () => {
  await authStore.loadSavedUser()
})

</script>

<style src="../node_modules/bulma/css/bulma.css"></style>
