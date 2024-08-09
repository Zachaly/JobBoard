<template>
    <ViewTemplate :company-navbar="true">
        <div class="columns is-centered">
            <div class="column is-3 is-centered">
                <p class="title">Name: {{ profile.name }}</p>
                <p class="subtitle">Contact email: {{ profile.contactEmail }}</p>
                <p class="subtitle">Country: {{ profile.country }}</p>
                <p class="subtitle">City: {{ profile.city }}</p>
                <p class="subtitle">Postal code: {{ profile.postalCode }}</p>
                <p class="subtitle">Address: {{ profile.address }}</p>
                <p class="subtitle">About us</p>
                <p>
                    {{ profile.about }}
                </p>
                <RouterLink to="/company/profile/update" class="button">Update</RouterLink>
                <RouterLink to="/company/profile/offers" class="button">Job offers</RouterLink>

                <div class="mt-1">
                    <div class="file">
                        <label class="file-label">
                          <input class="file-input" type="file" @change="changeFile"/>
                          <span class="file-cta">
                            <span class="file-label"> Choose a file… </span>
                          </span>
                        </label>
                      </div>
                    <button class="button" @click="updateProfilePicture()">Update profile picture</button>
                </div>
            </div>
            <div class="column is-2">
                <figure class="image is-128x128">
                    <img :src="$companyImage(profile?.id ?? 0)" alt="">
                </figure>
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import useAuthStore from '@/stores/AuthStore';
import { RouterLink } from 'vue-router';
import { Ref, ref } from 'vue';
import CompanyAccountModel from '../model/company-account/CompanyAccountModel';
import ViewTemplate from "@/views/ViewTemplate.vue";
import axios from 'axios';

const authStore = useAuthStore()

const profile: Ref<CompanyAccountModel> = ref(authStore.companyData!)

const currentFile = ref<File | null>()

const changeFile = (e: Event) => {
    const target = e.target as HTMLInputElement
    if(target != null) {
        currentFile.value = target.files![0]
    }
    
}

const updateProfilePicture = () => {

    const form = new FormData()

    form.append('CompanyId', profile.value.id.toString())
    if(currentFile.value != null) {
        form.append('Picture', currentFile.value)
    }

    axios.post('company-account/image', form).then(() => alert('Picture updated'))
}
</script>