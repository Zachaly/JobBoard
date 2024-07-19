<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns">
            <div class="column is-3">
                <input placeholder="Location" type="text" class="input" v-model="searchRequest.Location" />
                <input placeholder="Company Name" class="input" v-model="searchRequest.SearchCompanyName" />
                <p class="title">Businesses</p>   
                <div v-for="business in businesses" :key="business.id">
                    <label class="checkbox">
                        <input type="checkbox" class="checkbox" @change="changeBusinesses(business.id)">
                        {{ business.name }}
                    </label>
                </div>
                <button class="button is-info" @click="loadOffers()">Search</button>
            </div>
            <div class="column is-8">
                <p>
                    {{ offerCount }} offers found
                </p>
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
import BusinessModel from '../model/business/BusinessModel';

const offers = ref<JobOfferModel[]>([])
const offerCount = ref(0)
const businesses = ref<BusinessModel[]>([])

const searchRequest: Ref<GetJobOfferRequest> = ref({
    MinimalExpirationDate: new Date().toISOString(),
    BusinessIds: []
})

const changeBusinesses = (id: number) => {
    if (searchRequest.value.BusinessIds?.includes(id)) {
        searchRequest.value.BusinessIds = searchRequest.value.BusinessIds.filter(x => x != id)
    } else {
        searchRequest.value.BusinessIds?.push(id)
    }
}

const loadOffers = () => {
    axios.get('job-offer', { params: searchRequest.value }).then(res => offers.value = res.data)
    axios.get('job-offer/count', { params: searchRequest.value }).then(res => offerCount.value = res.data)
}

onMounted(() => {
    loadOffers()
    axios.get<BusinessModel[]>('business').then(res => businesses.value = res.data)
})
</script>