<template>
    <div class="card m-1 p-1">
        <div class="media">
            <div class="media-content">
                <RouterLink :to="`/application/${application.id}`" class="title">{{ application.employee.firstName }} {{ application.employee.lastName }}</RouterLink>
                <p class="subtitle">
                    <ApplicationState :state="application.state" />
                </p>
            </div>
            <div class="media-right">
                <p class="subtitle">{{ $fromUtcDate(application.applicationDate) }}</p>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';
import JobOfferApplicationModel from '../model/job-offer-application/JobOfferApplicationModel';
import ApplicationState from '@/components/ApplicationStateComponent.vue';
const props = defineProps<{
    application: JobOfferApplicationModel
}>()
</script>