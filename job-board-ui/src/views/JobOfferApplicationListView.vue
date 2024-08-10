<template>
    <ViewTemplate :company-navbar="true">
        <div class="columns is-centered">
            <div class="column is-6">
                <JobOfferApplication v-for="app in applications" :key="app.id" :application="app" />
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import JobOfferApplication from '@/components/JobOfferApplicationComponent.vue'
import { useRoute } from 'vue-router';
import { ref, onMounted } from 'vue';
import JobOfferApplicationModel from '../model/job-offer-application/JobOfferApplicationModel';
import axios from 'axios';
import GetJobOfferApplicationRequest from '../model/job-offer-application/GetJobOfferApplicationRequest';

const route = useRoute()

const applications = ref<JobOfferApplicationModel[]>([])

onMounted(() => {
    const params: GetJobOfferApplicationRequest = {
        OfferId: parseInt(route.params.id as string)
    }

    axios.get<JobOfferApplicationModel[]>('job-offer-application', { params }).then(res => applications.value = res.data)
})
</script>