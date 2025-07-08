<script setup lang="tsx">
import { ContentWrap } from '@/components/ContentWrap'
import { Search } from '@/components/Search'
import { Dialog } from '@/components/Dialog'
import { useI18n } from '@/hooks/web/useI18n'
import { ElTag } from 'element-plus'
import { Table } from '@/components/Table'
import { getUsersApi, deleteUsersApi, saveUserApi, getUserApi } from '@/api/users'
import { useTable } from '@/hooks/web/useTable'
import { TableData } from '@/api/table/types'
import { ref, unref, reactive } from 'vue'
import Write from './components/Write.vue'
import Detail from './components/Detail.vue'
import { CrudSchema, useCrudSchemas } from '@/hooks/web/useCrudSchemas'
import { BaseButton } from '@/components/Button'
import { useIcon } from '@/hooks/web/useIcon'

const ids = ref<string[]>([])

const { tableRegister, tableState, tableMethods } = useTable({
    fetchDataApi: async () => {
        const { currentPage, pageSize } = tableState
        const res = await getUsersApi({
            pageIndex: unref(currentPage),
            pageSize: unref(pageSize),
            ...unref(searchParams),
            ...unref(sortParams)
        })
        return res
    },
    fetchDelApi: async () => {
        const res = await deleteUsersApi(unref(ids))
        return !!res
    }
})
const { loading, dataList, total, currentPage, pageSize } = tableState
const { getList, getElTableExpose, delList } = tableMethods

const searchParams = ref({})
const setSearchParams = (params: any) => {
    searchParams.value = params
    getList()
}

const sortParams = ref({})
const setSortParams = (params: any) => {
    sortParams.value = params
    getList()
}

const { t } = useI18n()

const action = async (row: TableData, type: string) => {
    if (type === 'edit' || type === 'detail') {
        row = await getUserApi(row.id)
    }

    dialogTitle.value = t(type === 'edit' ? 'exampleDemo.edit' : 'exampleDemo.detail')
    actionType.value = type
    currentRow.value = row
    dialogVisible.value = true
}

const AddAction = () => {
    dialogTitle.value = t('exampleDemo.add')
    actionType.value = 'add'
    currentRow.value = null
    dialogVisible.value = true
}

const crudSchemas = reactive<CrudSchema[]>([
    {
        field: 'selection',
        search: {
            hidden: true
        },
        table: {
            type: 'selection'
        },
        form: {
            hidden: true
        }
    },
    {
        field: 'id',
        label: t('tableUsers.id'),
        fixed: true,
        search: {
            hidden: true
        },
        form: {
            hidden: true
        }
    },

    {
        field: 'login',
        label: t('tableUsers.login'),
        table: {
            hidden: true
        },
        editForm: {
            hidden: true
        },
        detailForm: {
            hidden: true
        }
    },
    {
        field: 'password',
        label: t('tableUsers.password'),
        table: {
            hidden: true
        },
        form: {
            component: 'InputPassword'
        },
        editForm: {
            hidden: true
        },
        detailForm: {
            hidden: true
        }
    },

    {
        field: 'lastName',
        label: t('tableUsers.lastName'),
        table: {
            hidden: true
        },
        addForm: {
            colProps: {
                span: 24
            }
        },
        editForm: {
            colProps: {
                span: 24
            }
        },
        detailForm: {
            hidden: true
        }
    },
    {
        field: 'firstName',
        label: t('tableUsers.firstName'),
        table: {
            hidden: true
        },
        addForm: {
            colProps: {
                span: 24
            }
        },
        editForm: {
            colProps: {
                span: 24
            }
        },
        detailForm: {
            hidden: true
        }
    },
    {
        field: 'middleName',
        label: t('tableUsers.middleName'),
        table: {
            hidden: true
        },
        addForm: {
            colProps: {
                span: 24
            }
        },
        editForm: {
            colProps: {
                span: 24
            }
        },
        detailForm: {
            hidden: true
        }
    },
    {
        field: 'name',
        label: t('tableUsers.name'),
        search: {
            hidden: true
        },
        addForm: {
            hidden: true
        },
        editForm: {
            hidden: true,
            colProps: {
                // TODO: Need fix hidden fields in base component
                span: 24
            }
        },
        detailForm: {
            span: 24
        }
    },
    {
        field: 'email',
        label: t('tableUsers.email'),
        addForm: {
            colProps: {
                span: 24
            }
        }
    },

    {
        field: 'phoneNumber',
        label: t('tableUsers.phone'),
        addForm: {
            hidden: true
        }
    },
    {
        field: 'blocked',
        label: t('tableUsers.blocked'),
        addForm: {
            hidden: true
        },
        editForm: {
            component: 'Switch'
        }
    },
    {
        field: 'blockedReason',
        label: t('tableUsers.blockedReason'),
        search: {
            hidden: true
        },
        table: {
            hidden: true
        },
        addForm: {
            hidden: true
        },
        editForm: {
            colProps: {
                span: 24
            }
        },
        detailForm: {
            span: 24
        }
    },
    {
        field: 'action',
        width: '210px',
        label: t('tableDemo.action'),
        fixed: 'right',
        sortable: false,
        search: {
            hidden: true
        },
        form: {
            hidden: true
        },
        table: {
            slots: {
                default: (data: any) => {
                    return (
                        <>
                            <BaseButton
                                type="primary"
                                icon={useIcon({ icon: 'vi-ep:edit' })}
                                onClick={() => action(data.row, 'edit')}
                            />
                            <BaseButton
                                type="success"
                                icon={useIcon({ icon: 'vi-ep:reading' })}
                                onClick={() => action(data.row, 'detail')}
                            />
                            <BaseButton
                                type="danger"
                                icon={useIcon({ icon: 'vi-ep:delete' })}
                                onClick={() => delData(data.row)}
                            />
                        </>
                    )
                }
            }
        }
    }
])

// @ts-ignore
const { allSchemas } = useCrudSchemas(crudSchemas)

const dialogVisible = ref(false)
const dialogTitle = ref('')

const currentRow = ref<TableData | null>(null)
const actionType = ref('')

const delLoading = ref(false)

const delData = async (row: TableData | null) => {
    const elTableExpose = await getElTableExpose()
    ids.value = row ? [row.id] : elTableExpose?.getSelectionRows().map((v: TableData) => v.id) || []
    delLoading.value = true
    await delList(unref(ids).length).finally(() => {
        delLoading.value = false
    })
}

const writeRef = ref<ComponentRef<typeof Write>>()

const saveLoading = ref(false)

const save = async () => {
    const write = unref(writeRef)
    const formData = await write?.submit()

    if (formData) {
        saveLoading.value = true
        const res = await saveUserApi(formData)
            .catch(() => {})
            .finally(() => {
                saveLoading.value = false
            })

        if (res) {
            dialogVisible.value = false

            if (!formData?.id) currentPage.value = 1

            getList()
        }
    }
}

const sortChange = async (column) => {
    if (column.order == null) {
        setSortParams(null)
        return
    }

    const sort = column.order === 'descending' ? `-${column.prop}` : column.prop
    setSortParams({ sort: sort })
}
</script>

<template>
    <ContentWrap>
        <!--<Search :schema="allSchemas.searchSchema" @search="setSearchParams" @reset="setSearchParams" />-->

        <div class="mb-10px">
            <BaseButton type="primary" @click="AddAction">
                {{ t('exampleDemo.add') }}
            </BaseButton>
            <BaseButton :loading="delLoading" type="danger" @click="delData(null)">
                {{ t('exampleDemo.del') }}
            </BaseButton>
        </div>

        <Table
            v-model:pageSize="pageSize"
            v-model:currentPage="currentPage"
            :columns="allSchemas.tableColumns"
            :data="dataList"
            :loading="loading"
            :sortable="true"
            :pagination="{ total: total }"
            :default-sort="{ prop: 'id', order: 'descending' }"
            @sort-change="sortChange"
            @register="tableRegister"
        />
    </ContentWrap>

    <Dialog v-model="dialogVisible" :title="dialogTitle">
        <Write
            v-if="actionType === 'add'"
            ref="writeRef"
            :form-schema="allSchemas.addFormSchema"
            :current-row="currentRow"
        />

        <Write
            v-if="actionType === 'edit'"
            ref="writeRef"
            :form-schema="allSchemas.editFormSchema"
            :current-row="currentRow"
        />

        <Detail
            v-if="actionType === 'detail'"
            :detail-schema="allSchemas.detailFormSchema"
            :current-row="currentRow"
        />

        <template #footer>
            <BaseButton
                v-if="actionType !== 'detail'"
                type="primary"
                :loading="saveLoading"
                @click="save"
            >
                {{ t('exampleDemo.save') }}
            </BaseButton>
            <BaseButton @click="dialogVisible = false">{{ t('dialogDemo.close') }}</BaseButton>
        </template>
    </Dialog>
</template>
