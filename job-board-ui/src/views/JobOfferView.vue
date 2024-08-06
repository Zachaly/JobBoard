<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns">
            <div class="column is-2"></div>
            <div class="column is-8">
                <JobOfferListItem :offer="offer" />
                <p>
                    {{ offer.description }}
                </p>
                <p class="title mt-2">
                    Requirements
                </p>
                <p class="subtitle mt-2" v-for="req in offer.requirements" :key="req.id">{{ req.content }}</p>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { onMounted, Ref, ref } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import axios from 'axios';
import JobOfferListItem from '@/components/JobOfferListItemComponent.vue';
import { useRoute } from 'vue-router';

const route = useRoute()

const offer: Ref<JobOfferModel> = ref({
    id: 0,
    title: '',
    description: '',
    location: '',
    expirationDate: '',
    creationDate: '',
    company: {
        id: 0,
        name: '',
        city: '',
        postalCode: '',
        contactEmail: '',
        country: '',
        address: ''
    },
    requirements: [],
    tags: []
})

onMounted(() => {
    axios.get(`job-offer/${route.params.id}`).then(res => offer.value = res.data)
})

</script>