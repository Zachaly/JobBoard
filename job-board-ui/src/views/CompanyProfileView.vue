<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-8 is-centered">
                <button class="button is-danger" @click="logout">Logout</button>
                <p class="title">Name: {{ profile.name }}</p>
                <p class="subtitle">Contact email: {{ profile.contactEmail }}</p>
                <p class="subtitle">Country: {{ profile.country }}</p>
                <p class="subtitle">City: {{ profile.city }}</p>
                <p class="subtitle">Postal code: {{ profile.postalCode }}</p>
                <p class="subtitle">Address: {{ profile.address }}</p>
                <RouterLink to="/company/profile/update" class="button">Update</RouterLink>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import useAuthStore from '@/stores/AuthStore';
import { useRouter, RouterLink } from 'vue-router';
import { Ref, ref } from 'vue';
import CompanyAccountModel from '../model/company-account/CompanyAccountModel';
import ViewTemplate from "@/views/ViewTemplate.vue";

const authStore = useAuthStore()
const router = useRouter()

const profile: Ref<CompanyAccountModel> = ref(authStore.companyData!)

const logout = () => {
    authStore.logout()
    router.push('/company/login')
}
</script>