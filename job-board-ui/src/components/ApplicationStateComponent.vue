<template>
    <span>
        {{ text }}
    </span>
</template>

<script setup lang="ts">
import JobOfferApplicationState from '@/model/enum/JobOfferApplicationState';
import { computed, defineProps } from 'vue';
const props = defineProps<{
    state: JobOfferApplicationState
}>()

const text = computed(() => {
    switch (props.state) {
        case JobOfferApplicationState.Sent:
            return 'Sent'
        case JobOfferApplicationState.Accepted:
            return 'Accepted'
        case JobOfferApplicationState.Opened:
            return 'Opened'
        case JobOfferApplicationState.Processed:
            return 'Processed'
        case JobOfferApplicationState.Rejected:
            return 'Rejected'
    }
    return ''
})
</script>