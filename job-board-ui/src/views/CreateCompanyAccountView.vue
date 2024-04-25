<template>
    <div class="columns is-centered">
        <div class="column is-6">
            <div class="control">
                <label class="label">Login email</label>
                <input type="text" v-model="request.email" class="input">
                <ValidationErrorsComponent :errors="validationErrors['Email']"/>
            </div>
            <div class="control">
                <label class="label">Password</label>
                <input type="password" v-model="request.password" class="input">
                <ValidationErrorsComponent :errors="validationErrors['Password']"/>
            </div>
            <div class="control">
                <label class="label">Company name</label>
                <input type="text" v-model="request.name" class="input">
                <ValidationErrorsComponent :errors="validationErrors['Name']"/>
            </div>
            <div class="control">
                <label class="label">Country</label>
                <input type="text" v-model="request.country" class="input"/>
                <ValidationErrorsComponent :errors="validationErrors['Country']"/>
            </div>
            <div class="control">
                <label class="label">City</label>
                <input type="text" v-model="request.city" class="input"/>
                <ValidationErrorsComponent :errors="validationErrors['City']"/>
            </div>
            <div class="control">
                <label class="label">Postal code</label>
                <input type="text" v-model="request.postalCode" class="input">
                <ValidationErrorsComponent :errors="validationErrors['PostalCode']"/>
            </div>
            <div class="control">
                <label class="label">Address</label>
                <input type="text" v-model="request.address" class="input">
                <ValidationErrorsComponent :errors="validationErrors['Address']"/>
            </div>
            <div class="control">
                <label class="label">Contact email</label>
                <input type="text" v-model="request.contactEmail" class="input">
                <ValidationErrorsComponent :errors="validationErrors['ContactEmail']"/>
            </div>
            <div class="control">
                <button @click="addAccount" class="button">Create account</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, ref } from 'vue';
import AddCompanyAccountRequest from '@/model/company-account/AddCompanyAccountRequest'
import axios, { AxiosError } from 'axios';
import { useRouter } from 'vue-router';
import ValidationErrorsComponent from '@/components/ValidationErrorsComponent.vue';
import ResponseModel from '@/model/ResponseModel';

const router = useRouter()

const validationErrors: Ref<{[id: string]: string[]}> = ref({})

const request: Ref<AddCompanyAccountRequest> = ref({
    email: '',
    password: '',
    city: '',
    address: '',
    name: '',
    postalCode: '',
    contactEmail: '',
    country: '',
})

const addAccount = () => {
    axios.post('company-account', request.value).then(() => {
        alert('Account created')
        router.push('/company')
    }).catch((err: AxiosError<ResponseModel>) => {
        if(err.response && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}
</script>