<template>
    <ViewTemplate :admin-navbar="true">
        <div class="columns is-centered">
            <div class="column is-6">
                <table class="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="business in businesses" :key="business.id">
                            <td>
                                {{ business.id }}
                            </td>
                            <td v-if="selectedBusiness?.id != business.id" @click="selectBusiness(business)">
                                {{ business.name }}
                            </td>
                            <td v-else>
                                <input class="input" type="text" v-model="selectedBusiness.name">
                            </td>
                            <td v-if="selectedBusiness?.id == business.id">
                                <button class="button" @click="update()">Update</button>
                            </td>
                            <td>
                               <button class="button is-danger" @click="deleteBusiness(business)">Delete</button> 
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="control">
                    <input type="text" class="input" v-model="newName">
                    <button class="button is-success" @click="addNew">Add new</button>
                </div>
                
            </div>
        </div>
    </ViewTemplate>
</template>

<script setup lang="ts">
import ViewTemplate from '@/views/ViewTemplate.vue';
import { Ref, ref, onMounted } from 'vue';
import BusinessModel from '../model/business/BusinessModel';
import axios from 'axios';
import PagedRequest from '../model/PagedRequest';
import AddBusinessRequest from '../model/business/AddBusinessRequest';
import UpdateBusinessRequest from '../model/business/UpdateBusinessRequest';

const businesses: Ref<BusinessModel[]> = ref([])
const newName = ref('')
const selectedBusiness: Ref<BusinessModel | null> = ref(null)

const loadBusinesses = () => {
    const params: PagedRequest = {
        SkipPagination: true
    }

    axios.get<BusinessModel[]>('business', { params }).then(res => businesses.value = res.data)
}

const addNew = () => {
    const request: AddBusinessRequest = {
        name: newName.value
    }

    axios.post('business', request).then(() => {
        loadBusinesses()
        newName.value = ''
    })
}

const selectBusiness = (business: BusinessModel) => {
    selectedBusiness.value = business
}

const update = () => {
    if(!selectedBusiness.value) {
        return
    }

    const request: UpdateBusinessRequest = {
        name: selectedBusiness.value.name,
        id: selectedBusiness.value.id
    }

    axios.put('business', request).then(() => {
        selectedBusiness.value = null
        loadBusinesses()
    })
}

const deleteBusiness = (business: BusinessModel) => {
    axios.delete(`business/${business.id}`).then(() => loadBusinesses())
}

onMounted(() => loadBusinesses())
</script>