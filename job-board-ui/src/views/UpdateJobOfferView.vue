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
                    <div class="select">
                        <select v-model="request.workType">
                            <option :value="JobOfferWorkType.Onsite">Onsite</option>
                            <option :value="JobOfferWorkType.Hybrid">Hybrid</option>
                            <option :value="JobOfferWorkType.Remote">Remote</option>
                        </select>
                    </div>
                </div>
                <div class="control">
                    <button class="button" @click="clearSalary">Clear</button>
                    <label class="label">Min salary</label>
                    <input class="input" v-model="request.minSalary" type="number"/>
                    <label class="label">Max salary</label>
                    <input class="input" v-model="request.maxSalary" type="number"/>
                    <div class="select">
                        <select v-model="request.salaryType">
                            <option :value="undefined"></option>
                            <option :value="SalaryType.Hourly">Hourly</option>
                            <option :value="SalaryType.Daily">Daily</option>
                            <option :value="SalaryType.Monthly">Monthly</option>
                            <option :value="SalaryType.Yearly">Yearly</option>
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
                <p class="title">Tags</p>
                <div>
                    <input type="text" class="input" v-model="newTag">
                    <button class="button" @click="addTag()">Add</button>
                </div>
                <div class="is-flex">
                    <div class="m-1" v-for="tag in tags" :key="tag.id" @click="deleteTag(tag)">
                        {{ tag.tag }}
                    </div>
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
import BusinessModel from '../model/business/BusinessModel';
import PagedRequest from '../model/PagedRequest';
import JobOfferTagModel from '../model/job-offer-tag/JobOfferTagModel';
import AddJobOfferTagRequest from '../model/job-offer-tag/AddJobOfferTagRequest';
import GetJobOfferTagRequest from '../model/job-offer-tag/GetJobOfferTagRequest';
import JobOfferWorkType from '../model/enum/JobOfferWorkType';
import SalaryType from '../model/enum/SalaryType';

const request: Ref<UpdateJobOfferRequest> = ref({
    id: 0,
    title: '',
    description: '',
    expirationTimestamp: 0,
    location: '',
    workType: JobOfferWorkType.Onsite
})

const businesses: Ref<BusinessModel[]> = ref([])

const requirements: Ref<JobOfferRequirementModel[]> = ref([])
const newRequirement = ref('')

const tags: Ref<JobOfferTagModel[]> = ref([])
const newTag = ref('')

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

const addTag = () => {
    if (tags.value.some(t => t.tag == newTag.value)) {
        return;
    }

    const tagRequest: AddJobOfferTagRequest = {
        offerId: request.value.id,
        tag: newTag.value
    }

    axios.post('job-offer-tag', tagRequest).then(() => {
        const params: GetJobOfferTagRequest = {
            SkipPagination: true,
            OfferId: request.value.id
        }
        axios.get<JobOfferTagModel[]>('job-offer-tag', { params }).then(res => {
            tags.value = res.data
            newTag.value = ''
        })
    })
}

const deleteTag = (tag: JobOfferTagModel) => {
    axios.delete(`job-offer-tag/${tag.id}`).then(() => tags.value = tags.value.filter(x => x.id !== tag.id))
}

const clearSalary = () => {
    request.value.minSalary = undefined
    request.value.maxSalary = undefined
    request.value.salaryType = undefined
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
            description: res.data.description,
            businessId: res.data.businessId,
            workType: res.data.workType,
            minSalary: res.data.minSalary,
            maxSalary: res.data.maxSalary,
            salaryType: res.data.salaryType
        }

        requirements.value = res.data.requirements
        tags.value = res.data.tags

        updateTimestamp()
    })

    const params: PagedRequest = {
        SkipPagination: true
    }

    axios.get('business', { params }).then(res => businesses.value = res.data)
})
</script>