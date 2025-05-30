<template>
  <div class="app-container">
    <Page ref="page" :config="config">
      <template v-slot:status="{scope}">
        <el-tag :type="scope.row.status | statusFilter">{{ $utils.dictionaryTran('article_status',scope.row.status) }}</el-tag>
      </template>
    </Page>
  </div>
</template>

<script>
// const conf =
export default {
  components: { },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: 'success',
        1: 'gray'
      }
      return statusMap[status]
    }
  },
    data() {

    return {
      config: {
        columns: [
          { label: 'ID', prop: 'id' },
          {
            label: 'Имя', prop: 'username', type: 'input',
            // value: '默认值',
            rules: { required: true },
            attr: {
              placeholder: 'ФИО пользователя'
            },
            search: {
              attr: {
                placeholder: 'ФИО пользователя'
              }
            }
          },
          {
            label: 'Телефон', prop: 'number_phone', type: 'input',
            attr: {
              placeholder: 'Номер телефона с +7'
            },
            search: {
              attr: {
                placeholder: 'Номер телефона с +7'
              }
            }
          },
          {
            label: 'Email', prop: 'email', type: 'input',
            // value: '默认值',
            rules: { required: true },
            attr: {
              placeholder: 'Адрес эл. почты'
            },
            search: {
              attr: {
                placeholder: 'Адрес эл. почты'
              }
            }
          },
          {
            label: 'Логин', prop: 'login', type: 'input',
            rules: { required: true },
            attr: {
              placeholder: 'Введите логин'
            },
            search: {
              attr: {
                placeholder: 'Введите логин'
              }
            }
          },
          {
            label: 'Пароль', prop: 'password', type: 'input',
            rules: { required: true },
            attr: {
              type: 'password',
              placeholder: 'Введите пароль'
            },
            search: {
              attr: {
                placeholder: 'Введите пароль'
              }
            }
          },
          {
            label: 'Роль', prop: 'role', type: 'select', rules: { required: true }, options: this.$dictionaries.user_role
          },
          { label: 'Блок', prop: 'block', type: 'switch'},
        ],
        operation: {
          buttons: [
            {
              label: 'Добавить', type: 'primary',
              click: () => {
                this.open()
              }
            },
            {
              label: 'Удалить', type: 'default',
              click: () => {
                this.del()
              },
              disabledRender: (rows) => {
                return !(rows.length && rows.every(o => o.status !== '1'))
              }
            }
          ]
        },
        form: {
          url: '/vue-admin-simple/article/save'
        },
        table: {
          url: '/vue-admin-simple/article/list',
          url_del: '/vue-admin-simple/article/del',
          actionColumn: {
            label: 'Действия', fixed: 'right', width: 150,
            buttons: [
              {
                label: 'Редактировать',
                type: 'text',
                auth: 'edit',
                click: row => {
                  this.open(row)
                }
              },
              {
                label: 'Удалить',
                type: 'text',
                auth: 'del',
                act: 'del',
                disabledRender: (row) => {
                  return row.status === '1'
                }
              }
            ]
          }
        }
      }
    }
  },
  created() {
  },
  methods: {
    save(data) {
      this.$store.dispatch('form/registr', data).then(() => {
        this.$refs.page.$refs.form.visible = false
        this.$refs.page.$refs.table.refresh()
      })
    },
    open(row) {
      console.log(row)
      this.$refs.page.$refs.form.open(row)
    },
    del() {
      this.$refs.page.del()
    }
  }
}
</script>
