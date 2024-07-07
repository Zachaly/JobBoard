<template>
    <ViewTemplate :company-navbar="true">
        <div class="columns">
            <div class="column is-8">
                <JobOfferManagementListItem v-for="offer in offers" :key="offer.id" :offer="offer"
                    @delete="deleteOffer(offer)" @update="updateOffer(offer)"/>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { Ref, ref, onMounted } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import JobOfferManagementListItem from '@/components/JobOfferManagementListItemCompontent.vue'
import useAuthStore from '@/stores/AuthStore';
import axios from 'axios';
import { useRouter } from 'vue-router';

const offers: Ref<JobOfferModel[]> = ref([])
const authStore = useAuthStore()
const router = useRouter()

const loadOffers = () => {
    const params = { CompanyId: authStore.currentUserId }
    axios.get('job-offer', { params }).then(res => offers.value = res.data)
}

const deleteOffer = (offer: JobOfferModel) => {
    axios.delete(`job-offer/${offer.id}`).then(() => loadOffers())
}

const updateOffer = (offer: JobOfferModel) => {
    router.push(`/company/update-offer/${offer.id}`)
}

onMounted(() => {
    loadOffers()
})
</script>