<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-4">
                <p v-for="resume in resumes" :key="resume.id">
                    <a :href="$employeeResume(resume.id)">{{ resume.name }}</a>
                    <button class="button is-danger" @click="deleteResume(resume.id)">Delete</button>
                </p>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { ref, onMounted } from 'vue';
import EmployeeResumeModel from '../model/employee-resume/EmployeeResumeModel'
import GetEmployeeResumeRequest from '../model/employee-resume/GetEmployeeResumeRequest'
import axios from 'axios'
import useAuthStore from '@/stores/AuthStore';

const authStore = useAuthStore()
const resumes = ref<EmployeeResumeModel[]>([])

const deleteResume = (id: number) => {
    axios.delete(`employee-resume/${id}`).then(() => resumes.value = resumes.value.filter(x => x.id !== id))
}

onMounted(() => {
    const params: GetEmployeeResumeRequest = {
        EmployeeId: authStore.currentUserId
    }

    axios.get<EmployeeResumeModel[]>('employee-resume', { params }).then(res => resumes.value = res.data)
})
</script>