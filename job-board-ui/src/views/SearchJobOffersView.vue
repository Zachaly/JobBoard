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
                <p class="title">Work type</p>
                <div class="select">
                    
                    <select v-model="searchRequest.WorkType">
                        <option :value="undefined"></option>
                        <option :value="JobOfferWorkType.Onsite">Onsite</option>
                        <option :value="JobOfferWorkType.Hybrid">Hybrid</option>
                        <option :value="JobOfferWorkType.Remote">Remote</option>
                    </select>
                </div>
                <p class="title">Tags</p>
                <input type="text" v-model="newTag" class="input">
                <button class="button" @click="addTag()">Add tag</button>
                <div class="is-flex">
                    <div class="m-1" v-for="tag in searchRequest.Tags!" :key="tag" @click="deleteTag(tag)">
                        {{ tag }}
                    </div>
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
import JobOfferWorkType from '../model/enum/JobOfferWorkType'

const offers = ref<JobOfferModel[]>([])
const offerCount = ref(0)
const businesses = ref<BusinessModel[]>([])

console.log(Object.entries(JobOfferWorkType))

const searchRequest: Ref<GetJobOfferRequest> = ref({
    MinimalExpirationDate: new Date().toISOString(),
    BusinessIds: [],
    Tags: []
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

const newTag = ref('')

const addTag = () => {
    if(!newTag.value) {
        return;
    }

    searchRequest.value.Tags?.push(newTag.value)
    newTag.value = ''
}

const deleteTag = (tag: string) => {
    searchRequest.value.Tags = searchRequest.value.Tags?.filter(x => x != tag)
}

onMounted(() => {
    loadOffers()
    axios.get<BusinessModel[]>('business').then(res => businesses.value = res.data)
})
</script>