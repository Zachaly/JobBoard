<template>
  <router-view :key="$route.path" />
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import useAuthStore, { AuthType } from './stores/AuthStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

onMounted(async () => {
  await authStore.loadSavedUser()

  switch (authStore.currentAuthType) {
    case AuthType.Admin:
      router.push('/admin')
      break
    case AuthType.Company:
      router.push('/company')
      break
    case AuthType.Employee:
      router.push('/employee')
      break
  }
})
</script>

<style src="../node_modules/bulma/css/bulma.css"></style>
