<template>
    <div class="card m-2">
        <div class="card-content">
            <div class="media">
                <div class="media-content">
                    <RouterLink :to="`/employee/application/${application.id}`" class="title">{{ application.offerTitle }}</RouterLink>
                    <p class="subtitle">{{ $fromUtcDate(application.applicationDate)}}</p>
                    <p class="subtitle"> <ApplicationState :state="application.state"/> </p>
                </div>
                <div class="media-right">
                    <button class="button is-danger" @click="$emit('delete', application.id)">
                        Withdraw application
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import ApplicationState from '@/components/ApplicationStateComponent.vue'
import { defineProps, defineEmits } from 'vue';
import JobOfferApplicationModel from '../model/job-offer-application/JobOfferApplicationModel';
const props = defineProps<{
    application: JobOfferApplicationModel
}>()

defineEmits(['delete'])
</script>