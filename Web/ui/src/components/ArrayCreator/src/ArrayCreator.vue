<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { ContentWrap } from '@/components/ContentWrap'
import { Dialog } from '@/components/Dialog'
import { ElSelect, ElOption, ElText } from 'element-plus'
import { propTypes } from '@/utils/propTypes'
import { getEntityValuesApi } from '@/api/entities'
import { BaseButton } from '@/components/Button'
import { useI18n } from '@/hooks/web/useI18n'
import { CrudSchema, useCrudSchemas } from '@/hooks/web/useCrudSchemas'

const { t } = useI18n()

const props = defineProps({
    entity: propTypes.string.def(''),
    sort: propTypes.string.def(null)
})

type Option = {
    id: number
    consumption: number
    pressure: number
    efficiency: number
}

const options = ref<Option[]>([])
const isDialogOpen = ref(false)
const editedOption = ref<Option | null>(null)
const newOption = ref<Option>({ id: 0, consumption: '', pressure: '', efficiency: '' })

onMounted(async () => {
    if (props.entity === '') return
    try {
        const values = await getEntityValuesApi(props.entity, props.sort)
        options.value = values.data
    } catch (error) {
        console.error('������ ��� ��������� ������:', error)
    }
})

const openDialog = (option: Option | null = null) => {
    editedOption.value = option ? { ...option } : null
    newOption.value = option
        ? { ...option }
        : { id: 0, consumption: '', pressure: '', efficiency: '' }
    isDialogOpen.value = true
}

const closeDialog = () => {
    isDialogOpen.value = false
    editedOption.value = null
}

const saveOption = () => {
    if (!editedOption.value) {
        const newOptionWithId = { ...newOption.value, id: Date.now() }
        options.value.push(newOptionWithId)
    } else {
        const index = options.value.findIndex((option) => option.id === editedOption.value!.id)
        if (index !== -1) {
            options.value[index] = { ...newOption.value }
        }
    }
    closeDialog()
}

const removeOption = (optionId: number) => {
    options.value = options.value.filter((option) => option.id !== optionId)
}
</script>

<template>
    <div>
        <BaseButton type="primary" @click="openDialog()">��������</BaseButton>
        <ul style="list-style-type: none; padding: 0">
            <li
                v-for="option in options"
                :key="option.id"
                style="
                    padding: 5px;
                    border: 1px solid #ccc;
                    margin-bottom: 5px;
                    cursor: pointer;
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                "
            >
                <span @click="openDialog(option)"
                    >{{ option.consumption }} ({{ option.pressure }})</span
                >
                <BaseButton type="danger" size="small" @click.stop="removeOption(option.id)"
                    >�������</BaseButton
                >
            </li>
        </ul>

        <ElDialog v-model="isDialogOpen" title="������������� �������" @close="closeDialog">
            <ElText v-model="newOption.consumption" placeholder="�������� ��������" />
            <ElText v-model="newOption.pressure" placeholder="��������" />
            <ElText v-model="newOption.efficiency" placeholder="��������" />
            <template #footer>
                <BaseButton @click="closeDialog">������</BaseButton>
                <BaseButton type="primary" @click="saveOption">���������</BaseButton>
            </template>
        </ElDialog>
    </div>
</template>
