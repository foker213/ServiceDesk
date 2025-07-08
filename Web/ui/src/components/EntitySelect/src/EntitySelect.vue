<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { ElSelect, ElOption } from 'element-plus'
import { propTypes } from '@/utils/propTypes'
import { getEntityValuesApi } from '@/api/entities'

const props = defineProps({
    entity: propTypes.string.def(''),
    sort: propTypes.string.def(null)
})

type Option = {
    id: number
    name: string
}

const value = ref<Option>()
const options = ref([])

onMounted(async () => {
    if (props.entityType === '') return
    const values = await getEntityValuesApi(props.entity, props.sort)
    options.value = values.data
})
</script>

<template>
    <ElSelect v-model="value" value-key="id">
        <ElOption v-for="option in options" :key="option.id" :label="option.name" :value="option" />
    </ElSelect>
</template>
