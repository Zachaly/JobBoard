<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns">
            <div class="column is-2"></div>
            <div class="column is-6">
                <JobOfferListItem :offer="offer" />
                <p>
                    {{ offer.description }}
                </p>
                <p class="title mt-2">
                    Requirements
                </p>
                <p class="subtitle mt-2" v-for="req in offer.requirements" :key="req.id">{{ req.content }}</p>
            </div>
            <div class="column is-2" v-if="isAuthorized">
                <div class="mt-1">
                    <div class="control">
                        <div class="file">
                            <label class="file-label">
                                <input class="file-input" type="file" @change="changeFile" />
                                <span class="file-cta">
                                    <span class="file-label"> Attach resume </span>
                                </span>
                            </label>
                        </div>
                    </div>
                    <div class="select">
                        <select v-model="selectedResumeId">
                            <option :value="undefined"></option>
                            <option v-for="resume in resumes" :key="resume.id" :value="resume.id">{{ resume.name }}
                            </option>
                        </select>
                    </div>
                    <button class="button" @click="apply">Apply</button>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { onMounted, Ref, ref } from 'vue';
import JobOfferModel from '../model/job-offer/JobOfferModel';
import axios from 'axios';
import JobOfferListItem from '@/components/JobOfferListItemComponent.vue';
import { useRoute } from 'vue-router';
import useAuthStore from '@/stores/AuthStore';
import { AuthType } from '../stores/AuthStore';
import GetEmployeeResumeRequest from '../model/employee-resume/GetEmployeeResumeRequest';
import EmployeeResumeModel from '../model/employee-resume/EmployeeResumeModel';
import JobOfferWorkType from '../model/enum/JobOfferWorkType';

const route = useRoute()
const authStore = useAuthStore()
const isAuthorized = authStore.currentAuthType == AuthType.Employee
const currentFile = ref<File | null>()
const resumes = ref<EmployeeResumeModel[]>([])
const selectedResumeId = ref<number>()

const offer: Ref<JobOfferModel> = ref({
    id: 0,
    title: '',
    description: '',
    location: '',
    expirationDate: '',
    creationDate: '',
    company: {
        id: 0,
        name: '',
        city: '',
        postalCode: '',
        contactEmail: '',
        country: '',
        address: ''
    },
    requirements: [],
    tags: [],
    workType: JobOfferWorkType.Onsite
})

const changeFile = (e: Event) => {
    const target = e.target as HTMLInputElement

    currentFile.value = target.files![0]
}

const apply = () => {
    if (!currentFile.value && !selectedResumeId.value) {
        alert("You must attach your resume")
        return;
    }
    const form = new FormData()

    form.append("EmployeeId", authStore.currentUserId.toString())
    form.append("OfferId", offer.value.id.toString())

    if (currentFile.value) {
        form.append("Resume", currentFile.value)
    }

    if (selectedResumeId.value) {
        form.append('ResumeId', selectedResumeId.value.toString())
    }

    console.log(selectedResumeId.value)

    axios.post("job-offer-application", form).then(() => alert("Application sent!"))
}

onMounted(() => {
    axios.get(`job-offer/${route.params.id}`).then(res => offer.value = res.data)

    if (isAuthorized) {
        const params: GetEmployeeResumeRequest = {
            EmployeeId: authStore.currentUserId
        }
        axios.get<EmployeeResumeModel[]>('employee-resume', { params }).then(res => resumes.value = res.data)
    }
})



</script>