<template>
    <ViewTemplate :employee-navbar="true">
        <div class="columns is-centered">
            <div class="column is-6">
                <div class="control">
                    <label for="" class="label">First name</label>
                    <input type="text" class="input" v-model="request.firstName">
                    <ValidationErrors :errors="validationErrors['FirstName']" />
                </div>
                <div class="control">
                    <label for="" class="label">Last name</label>
                    <input type="text" class="input" v-model="request.lastName">
                    <ValidationErrors :errors="validationErrors['LastName']" />
                </div>
                <div class="control">
                    <label for="" class="label">Country</label>
                    <input type="text" class="input" v-model="request.country">
                    <ValidationErrors :errors="validationErrors['Country']" />
                </div>
                <div class="control">
                    <label for="" class="label">City name</label>
                    <input type="text" class="input" v-model="request.city">
                    <ValidationErrors :errors="validationErrors['City']" />
                </div>
                <div class="control">
                    <label for="" class="label">Phone number</label>
                    <input type="text" class="input" v-model="request.phoneNumber">
                    <ValidationErrors :errors="validationErrors['PhoneNumber']" />
                </div>
                <div class="control">
                    <label for="" class="label">About me</label>
                    <textarea class="textarea" name="" id="" cols="30" rows="10" v-model="request.aboutMe"></textarea>
                    <ValidationErrors :errors="validationErrors['AboutMe']"/>
                </div>
                <div class="buttons">
                    <button class="button is-info" @click="update()">Update</button>
                    <button class="button is-warning" @click="cancel()">Cancel</button>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import useAuthStore from '@/stores/AuthStore';
import { Ref, ref } from 'vue';
import UpdateEmployeeAccountRequest from '../model/employee-account/UpdateEmployeeAccountRequest';
import ValidationErrors from "@/components/ValidationErrorsComponent.vue";
import { useRouter } from 'vue-router';
import axios from 'axios';
import { AxiosError } from 'axios';
import ResponseModel from '../model/ResponseModel';

const authStore = useAuthStore()
const account = authStore.employeeData!
const router = useRouter()

const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const request: Ref<UpdateEmployeeAccountRequest> = ref({
    id: account.id,
    firstName: account.firstName,
    lastName: account.lastName,
    aboutMe: account.aboutMe,
    phoneNumber: account.phoneNumber,
    country: account.country,
    city: account.city
})

const update = () => {
    axios.put('employee-account', request.value).then(async () => {
        await authStore.updateUserData()
        router.push('/employee/profile')
    }).catch((err: AxiosError<ResponseModel>) => {
        console.log(err)
        if(err.response?.data && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}

const cancel = () => {
    router.push('/employee/profile')
}
</script>