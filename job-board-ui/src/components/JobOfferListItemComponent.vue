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
                    <p class="subtitle is-5">{{ offer.location }} | {{ JobOfferWorkType[offer.workType] }}</p>
                    <p class="subtitle is-5" v-if="offer.minSalary || offer.maxSalary">{{ salary }}</p>
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
import { defineProps, computed } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import JobOfferWorkType from '../model/enum/JobOfferWorkType'
import SalaryType from '../model/enum/SalaryType';
const props = defineProps<{
    offer: JobOfferModel
}>()

const salary = computed(() => {
    if (!props.offer.minSalary && !props.offer.maxSalary) {
        return ''
    }

    let period = ''

    if (props.offer.salaryType == SalaryType.Hourly) {
        period = 'hour'
    } else if (props.offer.salaryType == SalaryType.Daily) {
        period = 'day'
    } else if (props.offer.salaryType == SalaryType.Monthly) {
        period = 'month'
    } else {
        period = 'year'
    }

    if (props.offer.minSalary && !props.offer.maxSalary) {
        return `${props.offer.minSalary} / ${period}`
    }

    if (!props.offer.minSalary && props.offer.maxSalary) {
        return `${props.offer.maxSalary} / ${period}`
    }

    return `${props.offer.minSalary} - ${props.offer.maxSalary} / ${period}`
})

</script>