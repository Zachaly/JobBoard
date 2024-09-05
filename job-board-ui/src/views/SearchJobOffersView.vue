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
                <p class="title">Work experience</p>
                <div>
                    <label class="checkbox">
                        <input type="checkbox" class="checkbox" @change="changeExperienceLevel(WorkExperienceLevel.None)">
                        None
                    </label>
                </div>
                <div>
                    <label class="checkbox">
                        <input type="checkbox" class="checkbox" @change="changeExperienceLevel(WorkExperienceLevel.Intern)">
                        Intern
                    </label>
                </div>
                <div>
                    <label class="checkbox">
                        <input type="checkbox" class="checkbox" @change="changeExperienceLevel(WorkExperienceLevel.Junior)">
                        Junior
                    </label>
                </div>
                <div>
                    <label class="checkbox">
                        <input type="checkbox" class="checkbox" @change="changeExperienceLevel(WorkExperienceLevel.Mid)">
                        Mid
                    </label>
                </div>
                <div>
                    <label class="checkbox">
                        <input type="checkbox" class="checkbox" @change="changeExperienceLevel(WorkExperienceLevel.Senior)">
                        Senior
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
                <Pages :pageCount="Math.ceil(offerCount / 5)" @change-page="changePage"/>
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
import WorkExperienceLevel from '../model/enum/WorkExperienceLevel';
import Pages from '@/components/PagesComponent.vue'

const offers = ref<JobOfferModel[]>([])
const offerCount = ref(0)
const businesses = ref<BusinessModel[]>([])

const searchRequest: Ref<GetJobOfferRequest> = ref({
    MinimalExpirationDate: new Date().toISOString(),
    BusinessIds: [],
    Tags: [],
    ExperienceLevel: [],
    PageIndex: 0,
    PageSize: 10
})

const changeBusinesses = (id: number) => {
    if (searchRequest.value.BusinessIds?.includes(id)) {
        searchRequest.value.BusinessIds = searchRequest.value.BusinessIds.filter(x => x != id)
    } else {
        searchRequest.value.BusinessIds?.push(id)
    }
}

const loadOffers = () => {
    searchRequest.value.PageIndex = 0
    axios.get('job-offer', { params: searchRequest.value }).then(res => offers.value = res.data)
    axios.get('job-offer/count', { params: searchRequest.value }).then(res => offerCount.value = res.data)
}

const changePage = (index: number) => {
    searchRequest.value.PageIndex = index
    axios.get('job-offer', { params: searchRequest.value }).then(res => offers.value = res.data)
}

const newTag = ref('')

const addTag = () => {
    if (!newTag.value) {
        return;
    }

    searchRequest.value.Tags?.push(newTag.value)
    newTag.value = ''
}

const deleteTag = (tag: string) => {
    searchRequest.value.Tags = searchRequest.value.Tags?.filter(x => x != tag)
}

const changeExperienceLevel = (exp: WorkExperienceLevel) => {
    if (searchRequest.value.ExperienceLevel?.includes(exp)) {
        searchRequest.value.ExperienceLevel = searchRequest.value.ExperienceLevel.filter(x => x != exp)
    } else {
        searchRequest.value.ExperienceLevel?.push(exp)
    }
}

onMounted(() => {
    loadOffers()
    axios.get<BusinessModel[]>('business').then(res => businesses.value = res.data)
})
</script>