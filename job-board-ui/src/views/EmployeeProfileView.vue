<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-8 is-centered">
                <button class="button is-danger" @click="logout">Logout</button>
                <p class="title">{{ profile.firstName }} {{ profile.lastName }}</p>
                <p class="subtitle">Email: {{ profile.email }} </p>
                <p class="subtitle">Location: {{ profile.city }}, {{ profile.country }}</p>
                <p class="subtitle">Phone number: {{ profile.phoneNumber }}</p>
                <p class="subtitle">{{ profile.aboutMe }}</p>
                <RouterLink to="/employee/profile/update" class="button is-link">Update</RouterLink>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import useAuthStore from '@/stores/AuthStore';
import { useRouter } from 'vue-router';
import ViewTemplate from '@/views/ViewTemplate.vue';
import { ref } from 'vue';
import { RouterLink } from "vue-router";

const authStore = useAuthStore()
const router = useRouter()

const profile = ref(authStore.employeeData!)

const logout = () => {
    authStore.logout()
    router.push('/employee/login')
}
</script>