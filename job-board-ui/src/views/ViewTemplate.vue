<template>
    <EmployeeNavbar v-if="employeeNavbar"/>
    <CompanyNavbar v-if="companyNavbar"/>
    <div class="content">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import useAuthStore from '@/stores/AuthStore';
import { AuthType } from '../stores/AuthStore';
import EmployeeNavbar from "@/components/EmployeeNavbarComponent.vue";
import CompanyNavbar from "@/components/CompanyNavbarComponent.vue";
import { defineProps } from 'vue';
const authStore = useAuthStore()

const props = defineProps<{
    companyNavbar?: boolean,
    employeeNavbar?: boolean
}>()
</script>