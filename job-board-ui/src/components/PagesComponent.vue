<template>
    <nav class="pagination is-centered">
        <button href="#" class="pagination-previous" v-if="currentPage != 1" @click="changePage(currentPage - 1)">Previous</button>
        <ul class="pagination-list ">
            <li v-for="page in showedPages" :key="page"
                :class="{ 'pagination-link': true, 'is-current': currentPage == page }" @click="changePage(page)">
                {{ page }}
            </li>
        </ul>
        <button href="#" class="pagination-next" v-if="currentPage != pageCount" @click="changePage(currentPage + 1)">Next page</button>
    </nav>
</template>

<script setup lang="ts">
import { defineProps, ref, defineEmits, computed } from 'vue';

const props = defineProps<{
    pageCount: number
}>()

const emit = defineEmits(['change-page'])

const currentPage = ref(1)

const showedPages = computed(() => {
    const res = [currentPage.value]

    if (currentPage.value + 1 <= props.pageCount) {
        res.push(currentPage.value + 1)
    }

    if (currentPage.value - 1 > 0) {
        res.push(currentPage.value - 1)
    }

    if (!res.includes(1)) {
        res.unshift(1)
    }

    if (!res.includes(props.pageCount)) {
        res.push(props.pageCount)
    }

    res.sort((a, b) => a - b)

    return res
})

const changePage = (page: number) => {
    currentPage.value = page
    emit('change-page', page - 1)
}
</script>