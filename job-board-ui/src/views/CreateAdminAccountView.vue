<template>
    <div class="columns is-centered">
        <div class="column is-6 is-centered">
            <div class="control">
                <label for="" class="label">Login</label>
                <input class="input" type="text" v-model="request.login">
                <ValidationErrors :errors="validationErrors['Login']" />
            </div>
            <div class="control">
                <label for="" class="label">Password</label>
                <input type="password" class="input" v-model="request.password">
                <ValidationErrors :errors="validationErrors['Password']" />
            </div>
            <div class="control">
                <button class="button is-info" @click="addAccount()">
                    Create
                </button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, ref } from 'vue';
import AddAdminAccountRequest from '../model/admin-account/AddAdminAccountRequest';
import axios from 'axios';
import { AxiosError } from 'axios';
import ResponseModel from '../model/ResponseModel';
import { useRouter } from 'vue-router';
import ValidationErrors from '@/components/ValidationErrorsComponent.vue';

const request: Ref<AddAdminAccountRequest> = ref({
    login: '',
    password: ''
})

const validationErrors: Ref<{ [id: string]: string[] }> = ref({})

const router = useRouter()

const addAccount = () => {
    axios.post('admin-account', request.value).then(() => {
        alert('account created')
        router.push('/admin')
    }).catch((err: AxiosError<ResponseModel>) => {
        if (err.response && err.response.data.validationErrors) {
            validationErrors.value = err.response.data.validationErrors
        }
    })
}
</script>