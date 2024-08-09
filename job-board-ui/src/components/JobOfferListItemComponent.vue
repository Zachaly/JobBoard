<template>
    <div class="card m-2">
        <div class="card-content">
            <div class="media">
                <div class="media-left">
                    <figure class="image is-64x64">
                        <img :src="$companyImage(offer.company.id)" alt="">
                    </figure>
                </div>
                <div class="media-content">
                    <RouterLink :to="`/job-offer/${offer.id}`" class="title is-3">
                        {{ offer.title }}
                    </RouterLink>
                    <p class="subtitle is-5">{{ offer.businessName }} - <span v-for="tag in offer.tags" :key="tag.id">{{
                        tag.tag }} &nbsp;</span></p>
                    <p class="subtitle is-5">{{ offer.location }}</p>
                    <p class="title is-3">Company: <RouterLink :to="`/employee/company/${offer.company.id}`">{{
                        offer.company.name }}</RouterLink>
                    </p>
                    <p class="subtitle is-5">Created: {{ $fromUtcDate(offer.creationDate) }}</p>
                    <p class="subtitle is-5">Expires: {{ $fromUtcDate(offer.expirationDate) }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
const props = defineProps<{
    offer: JobOfferModel
}>()

</script>