<template>
    <ViewTemplate>
        <div class="column is-8">
            <div class="control">
                <label for="" class="label">Title</label>
                <input type="text" class="input" v-model="request.title">
                <ValidationErrors :errors="validationErrors['Title']" />
            </div>
            <div class="control">
                <label for="" class="label">Location</label>
                <input type="text" class="input" v-model="request.location">
                <ValidationErrors :errors="validationErrors['Location']" />
            </div>
            <div class="control">
                <label for="" class="label">Description</label>
                <textarea name="" id="" cols="30" rows="10" class="textarea" v-model="request.description">

                </textarea>
                <ValidationErrors :errors="validationErrors['Description']" />
            </div>
            <div class="control">
                <label for="" class="label">Expiration date</label>
                <input type="date" @change="updateTimestamp()" v-model="currentExpirationDate">
            </div>
            <div class="control">
                <button class="button" @click="update()">Update</button>
                <button class="button" @click="() => router.back()">Cancel</button>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { Ref, ref, onMounted } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import useAuthStore from '@/stores/AuthStore';
import { useRouter, useRoute } from 'vue-router';
import axios from 'axios';
import UpdateJobOfferRequest from '../model/job-offer/UpdateJobOfferRequest';
import { AxiosError } from 'axios';
import ResponseModel from '../model/ResponseModel';

const request: Ref<UpdateJobOfferRequest> = ref({
    id: 0,
    title: '',
    description: '',
    expirationTimestamp: 0,
    location: ''
})

const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const currentExpirationDate = ref('')

const authStore = useAuthStore()

const router = useRouter()
const route = useRoute()

const updateTimestamp = () => {
    const date = new Date(currentExpirationDate.value)
    request.value.expirationTimestamp = date.getTime()
}

const update = () => {
    axios.put('job-offer', request.value).then(() => router.back()).catch((err: AxiosError<ResponseModel>) => {
        if (err.response?.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}

onMounted(() => {
    axios.get<JobOfferModel>(`job-offer/${route.params.id}`).then(res => {
        if (res.data.company.id != authStore.currentUserId) {
            router.back()
            return
        }

        const expiration = new Date(res.data.expirationDate)

        currentExpirationDate.value = expiration.toISOString().split('T')[0]

        request.value = {
            id: res.data.id,
            expirationTimestamp: 0,
            title: res.data.title,
            location: res.data.location,
            description: res.data.description
        }

        updateTimestamp()
    })
})
</script>