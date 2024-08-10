<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-8 is-centered">
                <p class="title">{{ profile.firstName }} {{ profile.lastName }}</p>
                <p class="subtitle">Email: {{ profile.email }} </p>
                <p class="subtitle">Location: {{ profile.city }}, {{ profile.country }}</p>
                <p class="subtitle">Phone number: {{ profile.phoneNumber }}</p>
                <p class="subtitle">{{ profile.aboutMe }}</p>
                <RouterLink to="/employee/profile/update" class="button is-link m-1">Update</RouterLink>
                <RouterLink to="/employee/profile/application" class="button is-link m-1">See your applications ({{ applicationCount }})</RouterLink>
            </div>
            
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import useAuthStore from '@/stores/AuthStore';
import ViewTemplate from '@/views/ViewTemplate.vue';
import { ref, onMounted } from 'vue';
import { RouterLink } from "vue-router";
import axios from 'axios';
import GetJobOfferApplicationRequest from '../model/job-offer-application/GetJobOfferApplicationRequest';

const authStore = useAuthStore()

const profile = ref(authStore.employeeData!)

const applicationCount = ref(0)

onMounted(() => {
    const params: GetJobOfferApplicationRequest = {
        EmployeeId: authStore.employeeData?.id
    }
    axios.get<number>('job-offer-application/count', { params }).then(res => applicationCount.value = res.data)
})
</script>