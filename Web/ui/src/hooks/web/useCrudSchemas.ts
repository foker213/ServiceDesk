import { reactive } from 'vue'
import { eachTree, treeMap, filter } from '@/utils/tree'
import { FormSchema } from '@/components/Form'
import { TableColumn } from '@/components/Table'
import { DescriptionsSchema } from '@/components/Descriptions'

export type CrudSchema = Omit<TableColumn, 'children'> & {
    search?: CrudSearchParams
    table?: CrudTableParams
    form?: CrudFormParams
    addForm?: CrudFormParams
    editForm?: CrudFormParams
    detail?: CrudDescriptionsParams
    detailForm?: CrudDescriptionsParams
    children?: CrudSchema[]
}

interface CrudSearchParams extends Omit<FormSchema, 'field'> {
    // 是否隐藏在查询项
    hidden?: boolean
}

interface CrudTableParams extends Omit<TableColumn, 'field'> {
    // 是否隐藏表头
    hidden?: boolean
}

interface CrudFormParams extends Omit<FormSchema, 'field'> {
    // 是否隐藏表单项
    hidden?: boolean
}

interface CrudDescriptionsParams extends Omit<DescriptionsSchema, 'field'> {
    // 是否隐藏表单项
    hidden?: boolean
}

interface AllSchemas {
    searchSchema: FormSchema[]
    tableColumns: TableColumn[]
    /**
     * @deprecated
     * Its old field for backward compability.
     * Use addFormSchema and editFormSchema instead formSchema.
     */
    formSchema: FormSchema[]
    addFormSchema: FormSchema[]
    editFormSchema: FormSchema[]
    /**
     * @deprecated
     * Its old field for backward compability.
     * Use detailFormSchema instead detailSchema.
     */
    detailSchema: DescriptionsSchema[]
    detailFormSchema: DescriptionsSchema[]
}

/**
 * @deprecated
 * It is not recommended to use.
 * It feels too cumbersome and not very flexible.
 * It may be removed in a certain version.
 */
export const useCrudSchemas = (
    crudSchema: CrudSchema[]
): {
    allSchemas: AllSchemas
} => {
    // 所有结构数据
    const allSchemas = reactive<AllSchemas>({
        searchSchema: [],
        tableColumns: [],
        formSchema: [],
        addFormSchema: [],
        editFormSchema: [],
        detailSchema: [],
        detailFormSchema: []
    })

    const searchSchema = filterSearchSchema(crudSchema)

    // @ts-ignore
    allSchemas.searchSchema = searchSchema || []

    const tableColumns = filterTableSchema(crudSchema)
    allSchemas.tableColumns = tableColumns || []

    const formSchema = filterFormSchema(crudSchema)
    const addFormSchema = filterAddFormSchema(crudSchema)
    const editFormSchema = filterEditFormSchema(crudSchema)

    allSchemas.formSchema = formSchema
    allSchemas.addFormSchema = addFormSchema
    allSchemas.editFormSchema = editFormSchema

    const detailSchema = filterDescriptionsSchema(crudSchema)
    const detailFormSchema = filterDetailFormSchema(crudSchema)

    allSchemas.detailSchema = detailSchema
    allSchemas.detailFormSchema = detailFormSchema

    return {
        allSchemas
    }
}

// 过滤 Search 结构
const filterSearchSchema = (crudSchema: CrudSchema[]): FormSchema[] => {
    const searchSchema: FormSchema[] = []
    const length = crudSchema.length

    for (let i = 0; i < length; i++) {
        const schemaItem = crudSchema[i]
        if (schemaItem.search?.hidden === true) {
            continue
        }
        // 判断是否隐藏
        const searchSchemaItem = {
            component: schemaItem?.search?.component || 'Input',
            ...schemaItem.search,
            field: schemaItem.field,
            label: schemaItem.search?.label || schemaItem.label
        }

        searchSchema.push(searchSchemaItem)
    }

    return searchSchema
}

// 过滤 table 结构
const filterTableSchema = (crudSchema: CrudSchema[]): TableColumn[] => {
    const tableColumns = treeMap<CrudSchema>(crudSchema, {
        conversion: (schema: CrudSchema) => {
            if (!schema?.table?.hidden) {
                return {
                    ...schema,
                    ...schema.table
                }
            }
        }
    })

    // 第一次过滤会有 undefined 所以需要二次过滤
    return filter<TableColumn>(tableColumns as TableColumn[], (data) => {
        if (data.children === void 0) {
            delete data.children
        }
        return !!data.field
    })
}

// 过滤 form 结构
const filterFormSchema = (crudSchema: CrudSchema[]): FormSchema[] => {
    const formSchema: FormSchema[] = []
    const length = crudSchema.length

    for (let i = 0; i < length; i++) {
        const formItem = crudSchema[i]
        const formSchemaItem = {
            component: formItem?.form?.component || 'Input',
            ...formItem.form,
            field: formItem.field,
            label: formItem.form?.label || formItem.label
        }

        formSchema.push(formSchemaItem)
    }

    return formSchema
}

const filterAddFormSchema = (crudSchema: CrudSchema[]): FormSchema[] => {
    const formSchema: FormSchema[] = []
    const length = crudSchema.length

    for (let i = 0; i < length; i++) {
        const formItem = crudSchema[i]
        if (formItem.form?.hidden || formItem.addForm?.hidden) continue

        const formSchemaItem = {
            component: formItem?.form?.component || 'Input',
            ...formItem.form,
            ...formItem.addForm,
            field: formItem.field,
            label: formItem.addForm?.label || formItem.label
        }

        formSchema.push(formSchemaItem)
    }

    return formSchema
}

const filterEditFormSchema = (crudSchema: CrudSchema[]): FormSchema[] => {
    const formSchema: FormSchema[] = []
    const length = crudSchema.length

    for (let i = 0; i < length; i++) {
        const formItem = crudSchema[i]
        if (formItem.form?.hidden || formItem.editForm?.hidden) continue

        const formSchemaItem = {
            component: formItem?.form?.component || 'Input',
            ...formItem.form,
            ...formItem.editForm,
            field: formItem.field,
            label: formItem.editForm?.label || formItem.label
        }

        formSchema.push(formSchemaItem)
    }

    return formSchema
}

// 过滤 descriptions 结构
const filterDescriptionsSchema = (crudSchema: CrudSchema[]): DescriptionsSchema[] => {
    const descriptionsSchema: FormSchema[] = []

    eachTree(crudSchema, (schemaItem: CrudSchema) => {
        // 判断是否隐藏
        if (!schemaItem?.detail?.hidden) {
            const descriptionsSchemaItem = {
                ...schemaItem.detail,
                field: schemaItem.field,
                label: schemaItem.detail?.label || schemaItem.label
            }

            // 删除不必要的字段
            delete descriptionsSchemaItem.hidden

            descriptionsSchema.push(descriptionsSchemaItem)
        }
    })

    return descriptionsSchema
}

const filterDetailFormSchema = (crudSchema: CrudSchema[]): DescriptionsSchema[] => {
    const descriptionsSchema: FormSchema[] = []

    eachTree(crudSchema, (schemaItem: CrudSchema) => {
        if (schemaItem?.form?.hidden || schemaItem?.detailForm?.hidden) return

        const descriptionsSchemaItem = {
            ...schemaItem.detailForm,
            field: schemaItem.field,
            label: schemaItem.detail?.label || schemaItem.label
        }

        delete descriptionsSchemaItem.hidden
        descriptionsSchema.push(descriptionsSchemaItem)
    })

    return descriptionsSchema
}
