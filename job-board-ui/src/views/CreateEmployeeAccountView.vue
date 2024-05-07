<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-6">
                <div class="control">
                    <label class="label">Email</label>
                    <input type="text" v-model="request.email" class="input">
                    <ValidationErrors :errors="validationErrors['Email']" />
                </div>
                <div class="control">
                    <label class="label">Password</label>
                    <input type="password" v-model="request.password" class="input">
                    <ValidationErrors :errors="validationErrors['Password']" />
                </div>
                <div class="control">
                    <label class="label">First name</label>
                    <input v-model="request.firstName" type="text" class="input" />
                    <ValidationErrors :errors="validationErrors['FirstName']" />
                </div>
                <div class="control">
                    <label class="label">Last name</label>
                    <input v-model="request.lastName" type="text" class="input" />
                    <ValidationErrors :errors="validationErrors['LastName']" />
                </div>
                <div class="control">
                    <label class="label">Phone number</label>
                    <input v-model="request.phoneNumber" type="text" class="input" />
                    <ValidationErrors :errors="validationErrors['PhoneNumber']" />
                </div>
                <div class="control">
                    <label class="label">Country</label>
                    <input v-model="request.country" class="input" type="text" />
                    <ValidationErrors :errors="validationErrors['Country']" />
                </div>
                <div class="control">
                    <label class="label">City</label>
                    <input v-model="request.city" type="text" class="input" />
                    <ValidationErrors :errors="validationErrors['City']" />
                </div>
                <div class="control">
                    <label class="label">About me</label>
                    <textarea class="textarea" cols="30" rows="10" v-model="request.aboutMe">
            </textarea>
                </div>
                <div class="control">
                    <button class="button is-success" @click="addAccount">Create account</button>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import { Ref, ref } from 'vue';
import ViewTemplate from './ViewTemplate.vue';
import AddEmployeeAccountRequest from '@/model/employee-account/AddEmployeeAccountRequest'
import axios, { AxiosError } from 'axios';
import ResponseModel from '@/model/ResponseModel';
import router from '@/router';
import ValidationErrors from '@/components/ValidationErrorsComponent.vue';

const request: Ref<AddEmployeeAccountRequest> = ref({
    email: '',
    password: '',
    lastName: '',
    firstName: '',
    phoneNumber: ''
})

const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const addAccount = () => {
    axios.post('employee-account', request.value).then(() => {
        alert('account created')
        router.push('/employee')
    }).catch((err: AxiosError<ResponseModel>) => {
        if (err.response && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}

</script>