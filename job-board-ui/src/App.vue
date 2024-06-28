<template>
  <router-view :key="$route.path" />
</template>

<script setup lang="ts">
import useAuthStore from './stores/AuthStore';

const authStore = useAuthStore()

authStore.loadSavedUser()

</script>

<style src="../node_modules/bulma/css/bulma.css"></style>
