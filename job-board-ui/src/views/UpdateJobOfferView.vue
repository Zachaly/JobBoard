<template>
    <ViewTemplate :company-navbar="true">
        <div class="columns">
            <div class="column is-6">
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
            <div class="column is-3">
                <p class="title">Requirements</p>
                <div>
                    <input type="text" class="input" v-model="newRequirement">
                    <button class="button" @click="addRequirement()">Add</button>
                </div>
                <div class="is-flex is-align-items-center is-justify-content-space-between" v-for="req in requirements"
                    :key="req.id">
                    {{ req.content }}
                    <button class="button is-warning" @click="deleteRequirement(req)">Delete</button>
                </div>
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
import JobOfferRequirementModel from '../model/job-offer-requirement/JobOfferRequirementModel';
import ValidationErrors from '@/components/ValidationErrorsComponent.vue';

const request: Ref<UpdateJobOfferRequest> = ref({
    id: 0,
    title: '',
    description: '',
    expirationTimestamp: 0,
    location: ''
})

const requirements: Ref<JobOfferRequirementModel[]> = ref([])
const newRequirement = ref('')

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

const addRequirement = () => {
    const requirementRequest = {
        offerId: request.value.id,
        content: newRequirement.value
    }

    axios.post('job-offer-requirement', requirementRequest).then(() => {
        axios.get<JobOfferRequirementModel[]>('job-offer-requirement', { params: { OfferId: request.value.id } })
            .then(res => requirements.value = res.data)
        newRequirement.value = ''
    })
}

const deleteRequirement = (req: JobOfferRequirementModel) => {
    axios.delete(`job-offer-requirement/${req.id}`)
        .then(() => requirements.value = requirements.value.filter(x => x.id != req.id))
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

        requirements.value = res.data.requirements

        updateTimestamp()
    })
})
</script>