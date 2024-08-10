<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-6">
                <EmployeeApplicationComponent v-for="app in applications" :key="app.id" :application="app" @delete="deleteApplication"/>
            </div>
        </div>
        
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import JobOfferApplicationModel from '../model/job-offer-application/JobOfferApplicationModel';
import { ref, onMounted } from 'vue';
import useAuthStore from '@/stores/AuthStore';
import axios from 'axios';
import GetJobOfferApplicationRequest from '../model/job-offer-application/GetJobOfferApplicationRequest';
import EmployeeApplicationComponent from '@/components/EmployeeApplicationComponent.vue';

const authStore = useAuthStore()

const applications = ref<JobOfferApplicationModel[]>([])

const deleteApplication = (id: number) => {
    const res = confirm('Are you sure?')
    if(!res) {
        return
    }

    axios.delete(`job-offer-application/${id}`).then(() => {
        applications.value = applications.value.filter(x => x.id != id)
    })
}

onMounted(() => {
    const params: GetJobOfferApplicationRequest = {
        EmployeeId: authStore.employeeData?.id
    }
    axios.get<JobOfferApplicationModel[]>('job-offer-application', { params }).then(res => applications.value = res.data)
})
</script>