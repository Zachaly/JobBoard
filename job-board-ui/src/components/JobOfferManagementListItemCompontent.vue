<template>
    <div class="card m-2">
        <div class="card-content">
            <div class="media">
                <div class="media-content">
                    <p class="title is-3">
                        {{ offer.title }}
                    </p>
                    <p class="subtitle is-5">{{ offer.businessName }}</p>
                    <p class="subtitle is-5">{{ offer.location }}</p>
                </div>
                <div class="media-right">
                    <div>
                        <button class="button is-danger" @click="$emit('delete')">Delete</button>
                        <button class="button is-warning" @click="$emit('update')">Update</button>
                    </div>
                    <div>
                        <RouterLink :to="`/job-offer/${offer.id}/applications`" class="button is-info">See applications</RouterLink>
                    </div>
                </div>
            </div>
            <div class="content">
                <p class="title is-3">Company: {{ offer.company.name }} </p>
                <p class="subtitle is-5">Created: {{ $fromUtcDate(offer.creationDate) }}</p>
                <p class="subtitle is-5">Expires: {{ $fromUtcDate(offer.expirationDate) }}</p>
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