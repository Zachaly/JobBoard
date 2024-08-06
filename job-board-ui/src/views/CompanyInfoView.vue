<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-8">
                <p class="title has-text-centered">{{ company?.name }}</p>
                <section class="section">
                    <h2 class="subtitle">
                        Address
                    </h2>
                    <p>
                        {{ company?.country }}, {{ company?.address }}, {{ company?.postalCode }} {{ company?.city }}
                    </p>
                </section>
                <section class="section">
                    <h2 class="subtitle">Contact</h2>
                    <p>
                        {{ company?.contactEmail }}
                    </p>
                </section>
                <section class="section">
                    <h2 class="subtitle">Job offers</h2>
                    <JobOfferListItem v-for="offer in jobOffers" :key="offer.id" :offer="offer"/>
                </section>
            </div>
        </div>
    </ViewTemplate>
</template>

<script lang="ts" setup>
import ViewTemplate from '@/views/ViewTemplate.vue';
import { ref, onMounted } from 'vue';
import CompanyAccountModel from '../model/company-account/CompanyAccountModel';
import axios from 'axios';
import { useRoute } from 'vue-router';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import GetJobOfferRequest from '../model/job-offer/GetJobOfferRequest';
import JobOfferListItem from '@/components/JobOfferListItemComponent.vue';

const company = ref<CompanyAccountModel>()
const jobOffers = ref<JobOfferModel[]>([])

const route = useRoute()

onMounted(() => {
    const params: GetJobOfferRequest = {
        CompanyId: parseInt(route.params.id as string ?? 0),
        MinimalExpirationDate: new Date().toISOString()
    }
    axios.get<CompanyAccountModel>(`company-account/${route.params.id}`).then(res => company.value = res.data)
    axios.get<JobOfferModel[]>("job-offer", { params }).then(res => jobOffers.value = res.data)
})
</script>