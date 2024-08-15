<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-8">
                <div class="control">
                    <label class="label">Your name</label>
                    <input type="text" v-model="request.name" class="input">
                </div>
                <div class="control">
                    <label for="" class="label">Email</label>
                    <input type="text" v-model="request.email" class="input">
                </div>
                <div class="control">
                    <label for="" class="label">Phone number</label>
                    <input type="text" v-model="request.phoneNumber" class="input">
                </div>
                <div class="control">
                    <label for="" class="label">City</label>
                    <input type="text" v-model="request.city" class="input">
                </div>
                <div class="control">
                    <label for="" class="label">About</label>
                    <textarea name="" id="" cols="30" rows="10" class="textarea" v-model="request.about"></textarea>
                </div>
                <div class="control">
                    <label for="" class="label">Work experience</label>
                    <div class="columns">
                        <div class="column">
                            <div v-for="exp in request.workExperience" :key="exp.startDate">
                                <p>{{ monthYearFromDateString(exp.startDate) }} - {{ monthYearFromDateString(exp.endDate) }}
                                </p>
                                <p>{{ exp.company }}</p>
                                <p>{{ exp.city }}</p>
                                <p>{{ exp.position }}</p>
                                <p>{{ exp.description }}</p>
                                <button class="button is-danger" @click="deleteWorkExperience(exp)">Delete</button>
                            </div>
                        </div>
                        <div class="column">
                            <input type="text" placeholder="Company" class="input" v-model="newWorkExperience.company">
                            <input type="text" placeholder="City" class="input" v-model="newWorkExperience.city">
                            <input type="text" placeholder="Position" class="input" v-model="newWorkExperience.position">
                            <input type="text" placeholder="Description" class="input"
                                v-model="newWorkExperience.description">
                            <label class="label">Start date</label>
                            <VueDatePicker v-model="newWorkExperienceStartDate" month-picker :dark="true" />
                            <label class="label">End date</label>
                            <VueDatePicker v-model="newWorkExperienceEndDate" month-picker :dark="true" />
                            <button class="button is-info" @click="addWorkExperience">Add</button>
                        </div>
                    </div>
                </div>
                <div class="control">
                    <label for="" class="label">Education</label>
                    <div class="columns">
                        <div class="column">
                            <div v-for="ed in request.education" :key="ed.startDate">
                                <p>{{ monthYearFromDateString(ed.startDate) }} - {{ monthYearFromDateString(ed.startDate) ??
                                    'now' }}</p>
                                <p>{{ ed.school }}</p>
                                <p>{{ ed.subject }}</p>
                                <p>{{ ed.level }}</p>
                                <button class="button is-danger" @click="deleteEducation(ed)">Delete</button>
                            </div>
                        </div>
                        <div class="column">
                            <input type="text" placeholder="School" class="input" v-model="newEducation.school">
                            <input type="text" placeholder="Subject" class="input" v-model="newEducation.subject">
                            <input type="text" placeholder="Level" class="input" v-model="newEducation.level">
                            <label class="label">Start date</label>
                            <VueDatePicker v-model="newEducationStartDate" month-picker :dark="true" />
                            <label class="label">End date</label>
                            <VueDatePicker v-model="newEducationEndDate" month-picker :dark="true" />
                            <button class="button is-info" @click="addEducation">Add</button>
                        </div>
                    </div>
                </div>
                <div class="control">
                    <label class="label">Languages</label>
                    <div class="columns">
                        <div class="column">
                            <p v-for="lang in request.languages" :key="lang.name">
                                {{ lang.name }}: {{ lang.proficiencyLevel }}
                                <button class="button is-danger" @click="deleteLanguage(lang)">Delete</button>
                            </p>
                        </div>
                        <div class="column">
                            <input class="input" v-model="newLanguage.name" placeholder="Name" />
                            <input class="input" v-model="newLanguage.proficiencyLevel" placeholder="Proficiency level" />
                            <button class="button is-success" @click="addLanguage">Add</button>
                        </div>
                    </div>
                </div>
                <div class="control">
                    <label class="label">Skills</label>
                    <input class="input" placeholder="Your skill" v-model="newSkill" />
                    <button class="button is-success" @click="addSkill">Add</button>
                    <p>
                        <span @click="deleteSkill(skill)" v-for="skill in request.skills" :key="skill">{{ skill }}, </span>
                    </p>
                </div>
                <div class="control">
                    <label class="label">Links</label>
                    <div class="columns">
                        <div class="column">
                            <p v-for="link in request.links" :key="link.link">
                                {{ link.description }} : {{ link.link }}
                                <button class="button is-danger" @click="deleteLink(link)">Delete</button>
                            </p>
                        </div>
                        <div class="column">
                            <input class="input" placeholder="Description" v-model="newLink.description" />
                            <input class="input" placeholder="Link" v-model="newLink.link" />
                            <button class="button is-success" @click="addLink">Add</button>
                        </div>
                    </div>
                </div>
                <div class="control">
                    <label class="label">Resume name</label>
                    <input class="input" v-model="request.resumeName" />
                </div>
                <div class="control">
                    <button class="button is-success" @click="addResume()">Add resume</button>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { ref } from 'vue';
import AddEmployeeResumeRequest from '../model/employee-resume/AddEmployeeResumeRequest';
import useAuthStore from '@/stores/AuthStore';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { ResumeEducation, ResumeWorkExperience, ResumeLink, ResumeLanguage } from '../model/employee-resume/AddEmployeeResumeRequest';

interface MonthYear {
    month: number,
    year: number
}

const authStore = useAuthStore()
const router = useRouter()

const employeeData = authStore.employeeData!

const request = ref<AddEmployeeResumeRequest>({
    name: `${employeeData.firstName} ${employeeData.lastName}`,
    resumeName: '',
    employeeId: employeeData.id,
    about: employeeData.aboutMe,
    skills: [],
    education: [],
    workExperience: [],
    languages: [],
    email: employeeData.email,
    phoneNumber: employeeData.phoneNumber,
    city: employeeData.city ?? '',
    links: []
})

const newSkill = ref('')
const newEducationStartDate = ref({ month: new Date().getMonth(), year: new Date().getFullYear() })
const newEducationEndDate = ref<MonthYear | undefined>()
const newEducation = ref<ResumeEducation>({
    subject: '',
    school: '',
    startDate: '',
    level: ''
})

const newWorkExperienceStartDate = ref({ month: new Date().getMonth(), year: new Date().getFullYear() })
const newWorkExperienceEndDate = ref<MonthYear | undefined>()
const newWorkExperience = ref<ResumeWorkExperience>({
    company: '',
    startDate: '',
    position: '',
    description: '',
    city: ''
})

const newLink = ref<ResumeLink>({
    description: '',
    link: ''
})

const newLanguage = ref<ResumeLanguage>({
    name: '',
    proficiencyLevel: ''
})

const monthYearFromDateString = (dateString: string | undefined) => {
    if (!dateString) {
        return 'now'
    }

    const date = new Date(dateString)
    const year = date.getFullYear()
    const month = date.getMonth()

    return `${month}.${year}`
}

const addResume = () => {
    axios.post('employee-resume', request.value).then(() => {
        alert('Resume created!')
        router.push('/employee/resume')
    }).catch(err => {
        console.log(err)
    })
}

const addLanguage = () => {
    if (!newLanguage.value.name || !newLanguage.value.proficiencyLevel) {
        return
    }
    request.value.languages.push(newLanguage.value)
    newLanguage.value = {
        name: '',
        proficiencyLevel: ''
    }
}

const deleteLanguage = (lang: ResumeLanguage) => {
    request.value.languages = request.value.languages.filter(x => x != lang)
}

const addEducation = () => {
    if (!newEducation.value.level || !newEducation.value.school || !newEducation.value.subject) {
        return
    }

    const startYear = newEducationStartDate.value.year
    const startMonth = newEducationStartDate.value.month

    newEducation.value.startDate = new Date(startYear, startMonth).toISOString()

    if (newEducationEndDate.value) {
        const endYear = newEducationEndDate.value.year
        const endMonth = newEducationEndDate.value.month

        newEducation.value.endDate = new Date(endYear, endMonth).toISOString()
    }

    request.value.education.push(newEducation.value)
    newEducation.value = {
        school: '',
        startDate: '',
        subject: '',
        level: ''
    }
}

const deleteEducation = (ed: ResumeEducation) => {
    request.value.education = request.value.education.filter(x => x != ed)
}

const addLink = () => {
    if (!newLink.value.description || !newLink.value.link) {
        return
    }

    request.value.links.push(newLink.value)

    newLink.value = {
        description: '',
        link: ''
    }
}

const deleteLink = (link: ResumeLink) => {
    request.value.links = request.value.links.filter(x => x != link)
}

const addWorkExperience = () => {
    if (!newWorkExperience.value.city || !newWorkExperience.value.company || !newWorkExperience.value.description
        || !newWorkExperience.value.position) {
        return
    }

    const startYear = newWorkExperienceStartDate.value.year
    const startMonth = newWorkExperienceStartDate.value.month + 1

    newWorkExperience.value.startDate = new Date(startYear, startMonth).toISOString()
    if (newWorkExperienceEndDate.value) {
        const endYear = newWorkExperienceEndDate.value.year
        const endMonth = newWorkExperienceEndDate.value.month + 1
        newWorkExperience.value.endDate = new Date(endYear, endMonth).toISOString()
    }

    request.value.workExperience.push(newWorkExperience.value)

    newWorkExperience.value = {
        company: '',
        position: '',
        startDate: '',
        city: '',
        description: ''
    }
}

const deleteWorkExperience = (exp: ResumeWorkExperience) => {
    request.value.workExperience = request.value.workExperience.filter(x => x != exp)
}

const addSkill = () => {
    if (!newSkill.value) {
        return
    }

    request.value.skills.push(newSkill.value)
    newSkill.value = ''
}

const deleteSkill = (skill: string) => {
    request.value.skills = request.value.skills.filter(x => x != skill)
}
</script>