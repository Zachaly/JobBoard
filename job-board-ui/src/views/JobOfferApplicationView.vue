<template>
    <ViewTemplate :company-navbar="true">
        <div class="columns is-centered">
            <div class="column is-6">
                <p class="title">{{ application?.employee.firstName }} {{ application?.employee.lastName }}</p>
                <p class="subtitle">{{ application?.employee.email }}</p>
                <p class="subtitle">{{ application?.employee.phoneNumber }}</p>
                <p class="subtitle">{{ application?.employee.city }}, {{ application?.employee.country }}</p>
                <p class="subtitle">{{ application?.employee.aboutMe }}</p>
                <p class="subtitle"><a :href="$resume(application?.id)" target="_blank">Resume</a></p>
                <p class="subtitle">{{ $fromUtcDate(application?.applicationDate) }}</p>
            </div>
            <div class="column is-2">
                <div class="select">
                    <select v-model="currentState" @change="updateState">
                        <option :value="JobOfferApplicationState.Opened">Opened</option>
                        <option :value="JobOfferApplicationState.Processed">Processed</option>
                        <option :value="JobOfferApplicationState.Accepted">Accepted</option>
                        <option :value="JobOfferApplicationState.Rejected">Rejected</option>
                    </select>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { useRoute } from 'vue-router';
import { ref, onMounted } from 'vue';
import JobOfferApplicationModel from '../model/job-offer-application/JobOfferApplicationModel';
import axios from 'axios';
import JobOfferApplicationState from '@/model/enum/JobOfferApplicationState';
import UpdateJobOfferApplicationRequest from '../model/job-offer-application/UpdateJobOfferApplicationRequest';

const route = useRoute()

const application = ref<JobOfferApplicationModel>({
    applicationDate: '',
    id: 0,
    offerId: 0,
    offerTitle: '',
    employee: {
        id: 0,
        lastName: '',
        firstName: '',
        phoneNumber: '',
        email: ''
    },
    state: 1
})

const currentState = ref<JobOfferApplicationState>(1)

const updateState = () => {
    const request: UpdateJobOfferApplicationRequest = {
        id: application.value.id,
        state: currentState.value
    }

    axios.put('job-offer-application', request)
}

onMounted(() => {
    axios.get<JobOfferApplicationModel>(`job-offer-application/${route.params.id}`).then(res => {
        application.value = res.data
        currentState.value = res.data.state
        if (res.data.state == JobOfferApplicationState.Sent) {
            const updateRequest: UpdateJobOfferApplicationRequest = {
                id: res.data.id,
                state: JobOfferApplicationState.Opened
            }

            axios.put('job-offer-application', updateRequest).then(() => {
                application.value!.state = JobOfferApplicationState.Opened
                currentState.value = JobOfferApplicationState.Opened
            })
        }
    })
})
</script>