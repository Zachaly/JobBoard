<template>
    <EmployeeNavbar v-if="employeeNavbar"/>
    <CompanyNavbar v-if="companyNavbar"/>
    <AdminNavbar v-if="adminNavbar"/>
    <div>
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import EmployeeNavbar from "@/components/EmployeeNavbarComponent.vue";
import CompanyNavbar from "@/components/CompanyNavbarComponent.vue";
import AdminNavbar from '@/components/AdminNavbarComponent.vue';
import { defineProps } from 'vue';

const props = defineProps<{
    companyNavbar?: boolean,
    employeeNavbar?: boolean,
    adminNavbar?: boolean
}>()
</script>