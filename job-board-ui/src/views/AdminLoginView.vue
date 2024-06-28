<template>
    <div class="columns is-centered">
        <div class="column is-6 is-centered">
            <div class="control">
                <label for="" class="label">Login</label>
                <input type="text" class="input" v-model="request.login">
            </div>
            <div class="control">
                <label for="" class="label">Password</label>
                <input type="password" class="input" v-model="request.password">
            </div>
            <label class="checkbox">
                <input type="checkbox" v-model="rememberMe">
                Remember me
            </label>
            <div class="control">
                <button class="button is-info" @click="login()">Login</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, ref } from 'vue';
import LoginRequest from '../model/LoginRequest';
import axios from 'axios';
import LoginResponse from '@/model/LoginResponse';
import useAuthStore, { AuthType } from '@/stores/AuthStore';
import { useRouter } from 'vue-router';

const request: Ref<LoginRequest> = ref({
    password: '',
    login: ''
})

const rememberMe = ref(false)

const authStore = useAuthStore()
const router = useRouter()

const login = () => {
    axios.post<LoginResponse>('admin-account/login', request.value).then(res => {
        authStore.authorize(res.data, AuthType.Admin, rememberMe.value)
        router.push('/admin')
    }).catch(() => {
        alert('Invalid login or password')
    })
}

</script>