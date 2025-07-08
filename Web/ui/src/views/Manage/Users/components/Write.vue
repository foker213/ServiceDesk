<script setup lang="ts">
import { Form, FormSchema } from '@/components/Form'
import { useForm } from '@/hooks/web/useForm'
import { PropType, reactive, watch } from 'vue'
import { TableData } from '@/api/table/types'
import { useI18n } from '@/hooks/web/useI18n'
import { useValidator } from '@/hooks/web/useValidator'

const { t } = useI18n()

const { required, email, minlength } = useValidator()

const props = defineProps({
    currentRow: {
        type: Object as PropType<Nullable<TableData>>,
        default: () => null
    },
    formSchema: {
        type: Array as PropType<FormSchema[]>,
        default: () => []
    }
})

const rules = reactive({
    lastName: [required()],
    firstName: [required()],
    middleName: [required()],
    login: [required()],
    password: [required(), minlength(8, t('rule.passwordMinLength'))],
    email: [required(), email(t('rule.email'))]
})

const { formRegister, formMethods } = useForm()
const { setValues, getFormData, getElFormExpose } = formMethods

const submit = async () => {
    const elForm = await getElFormExpose()
    const valid = await elForm?.validate().catch((err) => {
        console.log(err)
    })
    if (valid) {
        const formData = await getFormData()
        return formData
    }
}

watch(
    () => props.currentRow,
    (currentRow) => {
        if (!currentRow) return
        setValues(currentRow)
    },
    {
        deep: true,
        immediate: true
    }
)

defineExpose({
    submit
})
</script>

<template>
    <Form :rules="rules" @register="formRegister" :schema="formSchema" />
</template>
