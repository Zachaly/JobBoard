<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-8">
                <div class="control">
                    <label for="" class="label">Title</label>
                    <input type="text" class="input" v-model="request.title">
                    <ValidationErrors :errors="validationErrors['Title']"/>
                </div>
                <div class="control">
                    <label for="" class="label">Location</label>
                    <input type="text" class="input" v-model="request.location">
                    <ValidationErrors :errors="validationErrors['Location']"/>
                </div>
                <div class="control">
                    <label for="" class="label">Description</label>
                    <textarea name="" id="" cols="30" rows="10" class="textarea" v-model="request.description">

                    </textarea>
                    <ValidationErrors :errors="validationErrors['Description']"/>
                </div>
                <div class="control">
                    <label for="" class="label">Expiration date</label>
                    <input type="date" @change="getTimestamp()" v-model="currentDate">
                </div>
                <div class="control">
                    <button class="button" @click="add()">Add</button>
                    <button class="button" @click="() => router.back()">Cancel</button>
                </div>
            </div>
        </div>
    </ViewTemplate>    
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { Ref, ref } from 'vue';
import AddJobOfferRequest from '../model/job-offer/AddJobOfferRequest';
import useAuthStore from '@/stores/AuthStore';
import ValidationErrors from '@/components/ValidationErrorsComponent.vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { AxiosError } from 'axios';
import ResponseModel from '../model/ResponseModel';

const authStore = useAuthStore()
const companyId = authStore.companyData?.id ?? 0

const router = useRouter()

const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const request: Ref<AddJobOfferRequest> = ref({
    companyId,
    title: '',
    description: '',
    location: '',
    expirationTimestamp: 0
})

const currentDate = ref('')

const getTimestamp = () => {
    request.value.expirationTimestamp = new Date(currentDate.value).getTime()
}

const add = () => {
    axios.post('job-offer', request.value).then(() => {
        alert('Offer added')
        router.back()
    }).catch((err: AxiosError<ResponseModel>) => {
        if(err.response?.data && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}

</script>