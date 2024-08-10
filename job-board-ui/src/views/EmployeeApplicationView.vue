<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-6">
                <JobOfferListItem :offer="offer" v-if="offer"/>
                <p class="title">Your application</p>
                <p class="subtitle">Date: {{ $fromUtcDate(application?.applicationDate)}}</p>
                <p><a :href="$resume(application?.id)" target="_blank">Your cv</a></p>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { useRoute } from 'vue-router';
import JobOfferApplicationModel from '../model/job-offer-application/JobOfferApplicationModel';
import { ref, onMounted } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import axios from 'axios';
import JobOfferListItem from '@/components/JobOfferListItemComponent.vue';

const route = useRoute()

const application = ref<JobOfferApplicationModel>()
const offer = ref<JobOfferModel>()

onMounted(() => {
    axios.get<JobOfferApplicationModel>(`job-offer-application/${route.params.id}`).then(res => {
        application.value = res.data

        axios.get<JobOfferModel>(`job-offer/${res.data.offerId}`).then(res => offer.value = res.data)
    })
})
</script>