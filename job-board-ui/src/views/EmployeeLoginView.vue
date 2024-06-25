<template>
    <div class="columns is-centered">
        <div class="column is-6 is-centered">
            <div class="control">
                <label for="" class="label">Email</label>
                <input type="text" class="input" v-model="request.login">
            </div>
            <div class="control">
                <label for="" class="label">Password</label>
                <input type="password" class="input" v-model="request.password">
            </div>
            <div class="control">
                <button class="button is-info" @click="login()">Login</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import LoginRequest from '@/model/LoginRequest';
import LoginResponse from '@/model/LoginResponse';
import useAuthStore, { AuthType } from '@/stores/AuthStore';
import axios from 'axios';
import { Ref, ref } from 'vue';
import { useRouter } from 'vue-router';

const request: Ref<LoginRequest> = ref({
    password: '',
    login: ''
})

const authStore = useAuthStore()
const router = useRouter()

const login = () => {
    axios.post<LoginResponse>('employee-account/login', request.value).then(res => {
        authStore.authorize(res.data, AuthType.Employee)
        router.push('/employee')
    }).catch(() => {
        alert('Invalid email or password!')
    })
}

</script>