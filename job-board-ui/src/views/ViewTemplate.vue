<template>
    <EmployeeNavbar v-if="employeeNavbar"/>
    <CompanyNavbar v-if="companyNavbar"/>
    <div>
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import EmployeeNavbar from "@/components/EmployeeNavbarComponent.vue";
import CompanyNavbar from "@/components/CompanyNavbarComponent.vue";
import { defineProps } from 'vue';

const props = defineProps<{
    companyNavbar?: boolean,
    employeeNavbar?: boolean
}>()
</script>