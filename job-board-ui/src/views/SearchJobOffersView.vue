<template>
    <ViewTemplate>
        <div class="columns">
            <div class="column is-8">
                <div>
                    <input placeholder="Location" type="text" class="input" v-model="currentSearchValue">
                    <button class="button is-info" @click="loadOffers()">Search</button>
                </div>
                <JobOfferListItem v-for="offer in offers" :key="offer.id" :offer="offer" />
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import JobOfferListItem from '@/components/JobOfferListItemComponent.vue'
import { ref, onMounted, Ref } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import axios from 'axios';
import GetJobOfferRequest from '../model/job-offer/GetJobOfferRequest';

const offers = ref<JobOfferModel[]>([])

const currentSearchValue: Ref<string | undefined> = ref()

const loadOffers = () => {
    const params: GetJobOfferRequest = {
        Location: currentSearchValue.value
    }

    axios.get('job-offer', { params }).then(res => offers.value = res.data)
}

onMounted(() => {
    loadOffers()
})

</script>