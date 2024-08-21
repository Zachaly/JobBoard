<template>
    <ViewTemplate :company-navbar="true">
        <div class="columns is-centered">
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
                    <input type="date" @change="getTimestamp()" v-model="currentDate">
                </div>
                <div class="control">
                    <div class="select">
                        <select v-model="request.workType">
                            <option :value="JobOfferWorkType.Onsite">Onsite</option>
                            <option :value="JobOfferWorkType.Hybrid">Hybrid</option>
                            <option :value="JobOfferWorkType.Remote">Remote</option>
                        </select>
                    </div>
                </div>
                <div class="control">
                    <div class="select">
                        <select v-model="request.businessId">
                            <option :value="undefined">None</option>
                            <option v-for="business in businesses" :key="business.id" :value="business.id">{{ business.name
                            }}
                            </option>
                        </select>
                    </div>
                </div>

                <div class="control">
                    <button class="button" @click="add()">Add</button>
                    <button class="button" @click="() => router.back()">Cancel</button>
                </div>
            </div>
            <div class="column is-3">
                <p class="title">Requirements</p>
                <div>
                    <input type="text" class="input" v-model="newRequirement">
                    <button class="button" @click="addRequirement()">Add</button>
                </div>
                <div class="is-flex is-align-items-center is-justify-content-space-between"
                    v-for="req in request.requirements" :key="req">
                    {{ req }}
                    <button class="button is-warning" @click="deleteRequirement(req)">Delete</button>
                </div>
                <p class="title">Tags</p>
                <div>
                    <input type="text" class="input" v-model="newTag">
                    <button class="button" @click="addTag()">Add</button>
                </div>
                <div class="is-flex">
                    <div class="m-1" v-for="tag in request.tags" :key="tag" @click="deleteTag(tag)">{{ tag }}</div>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { Ref, ref, onMounted } from 'vue';
import AddJobOfferRequest from '../model/job-offer/AddJobOfferRequest';
import useAuthStore from '@/stores/AuthStore';
import ValidationErrors from '@/components/ValidationErrorsComponent.vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { AxiosError } from 'axios';
import ResponseModel from '../model/ResponseModel';
import BusinessModel from '../model/business/BusinessModel';
import PagedRequest from '../model/PagedRequest';
import JobOfferWorkType from '../model/enum/JobOfferWorkType';

const authStore = useAuthStore()
const companyId = authStore.companyData?.id ?? 0

const router = useRouter()

const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const newRequirement = ref('')
const newTag = ref('')

const businesses: Ref<BusinessModel[]> = ref([])

const request: Ref<AddJobOfferRequest> = ref({
    companyId,
    title: '',
    description: '',
    location: '',
    expirationTimestamp: 0,
    requirements: [],
    businessId: undefined,
    tags: [],
    workType: JobOfferWorkType.Onsite
})

const currentDate = ref('')

const getTimestamp = () => {
    request.value.expirationTimestamp = new Date(currentDate.value).getTime()
}

const addTag = () => {
    if (request.value.tags.includes(newTag.value)) {
        return;
    }

    request.value.tags.push(newTag.value)
    newTag.value = ''
}

const deleteTag = (tag: string) => {
    request.value.tags = request.value.tags.filter(x => x !== tag);
}

const addRequirement = () => {
    request.value.requirements.push(newRequirement.value)
    newRequirement.value = ''
}

const deleteRequirement = (req: string) => {
    request.value.requirements = request.value.requirements.filter(x => x != req)
}

const add = () => {
    axios.post('job-offer', request.value).then(() => {
        alert('Offer added')
        router.back()
    }).catch((err: AxiosError<ResponseModel>) => {
        if (err.response?.data && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}

onMounted(() => {
    const params: PagedRequest = {
        SkipPagination: true
    }
    axios.get('business', { params }).then(res => businesses.value = res.data)
})
</script>