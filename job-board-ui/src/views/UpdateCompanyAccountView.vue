<template>
    <ViewTemplate>
        <div class="columns is-centered">
            <div class="column is-6 is-centered">
                <div class="control">
                    <label for="" class="label">Name</label>
                    <input type="text" class="input" v-model="request.name">
                    <ValidationErrors :errors="validationErrors['Name']" />
                </div>
                <div class="control">
                    <label for="" class="label">Contact email</label>
                    <input type="text" class="input" v-model="request.contactEmail">
                    <ValidationErrors :errors="validationErrors['ContactEmail']" />
                </div>
                <div class="control">
                    <label for="" class="label">Country</label>
                    <input type="text" class="input" v-model="request.country">
                    <ValidationErrors :errors="validationErrors['Country']" />
                </div>
                <div class="control">
                    <label for="" class="label">City</label>
                    <input type="text" class="input" v-model="request.city">
                    <ValidationErrors :errors="validationErrors['City']" />
                </div>
                <div class="control">
                    <label for="" class="label">Postal code</label>
                    <input type="text" class="input" v-model="request.postalCode">
                    <ValidationErrors :errors="validationErrors['PostalCode']" />
                </div>
                <div class="control">
                    <label for="" class="label">Address</label>
                    <input type="text" class="input" v-model="request.address">
                    <ValidationErrors :errors="validationErrors['Address']" />
                </div>
                <div class="buttons">
                    <button class="button is-info" @click="update()">Confirm</button>
                    <button class="button is-warning" @click="cancel()">Cancel</button>
                </div>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import useAuthStore from '@/stores/AuthStore';
import UpdateCompanyAccountRequest from '../model/company-account/UpdateCompanyAccountRequest';
import { Ref, ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';
import { AxiosError } from 'axios';
import ResponseModel from '../model/ResponseModel';
import ValidationErrors from '@/components/ValidationErrorsComponent.vue';

const authStore = useAuthStore()
const account = authStore.companyData!
const router = useRouter()
const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const request: Ref<UpdateCompanyAccountRequest> = ref({
    id: account.id,
    address: account.address,
    postalCode: account.postalCode,
    name: account.name,
    contactEmail: account.contactEmail,
    country: account.country,
    city: account.city
})

const update = () => {
    axios.put('company-account', request.value).then(async () => {
        await authStore.updateUserData()
        router.push('/company/profile')
    }).catch((err: AxiosError<ResponseModel>) => {
        if (err.response?.data && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}

const cancel = () => {
    router.push('/company/profile')
}

</script>