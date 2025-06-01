<template>
  <div>
    <el-table ref="table"
              v-loading="$store.state.loading"
              :height="$props.height || innerHeight"
              v-bind="$props"
              :data="dataRender"
              @selection-change="selectionChange"
              @row-click="rowClick"
              v-on="$listeners">
      <template v-for="(item, index) in columnsRender">
        <el-table-column :key="index" :sortable="sortable" v-bind="item">
          <template v-if="item.slotScope" v-slot="{row, $index}">
            <slot v-if="item.slotScope" :name="item.slotScope" :scope="{row, $index}" />
          </template>
          <template v-else-if="item.dictionary" slot-scope="{row}">
            {{ $utils.dictionaryTran(item.dictionary, row[item.prop]) }}
          </template>
          <template v-else-if="item.propName" slot-scope="{row}">
            {{ row[item.propName] }}
          </template>
          <template v-else-if="item.type === 'upload'" slot-scope="{row}">
            <div v-if="row[item.prop]" style="line-height:0">
              <el-avatar shape="square" :size="30" :src="row[item.prop][0] && row[item.prop][0].url" @click.native="handlePictureCardPreview" />
            </div>
          </template>
          <template v-else-if="item.buttons" v-slot="{row, $index}">
            <el-button v-for="(btn, idx) in item.buttons"
                       :key="idx"
                       :disabled="btn.disabledRender ? btn.disabledRender(row) : btn.disabled"
                       v-bind="btn"
                       @click.stop="btn.act ? act(btn.act, row, $index) : btn.click(row, $index)">
              {{ btn.label }}
            </el-button>
          </template>
        </el-table-column>
      </template>
    </el-table>
    <pagination v-show="total > 0"
                :total="total"
                :page.sync="page"
                :limit.sync="limit"
                v-on="$listeners"
                @pagination="refresh" />

    <el-dialog title="图片预览" :visible.sync="dialogVisible" top="60px" append-to-body>
      <img width="100%" :src="dialogImageUrl" alt="">
    </el-dialog>
  </div>
</template>

<script>
  import Pagination from '@/components/Pagination'
  import { Table } from 'element-ui'
  import { getList } from '@/api/table'

  export default {
    name: 'MTable',
    components: { Pagination },
    props: Object.assign({ ...Table.props }, {
      columns: {
        type: Array,
        default: () => []
      },
      border: {
        type: Boolean,
        default: true
      },
      sortable: {
        type: Boolean,
        default: false
      },
      rowKey: {
        type: [String, Function],
        default: 'id'
      },
      initLoad: {
        type: Boolean,
        default: true
      },
      url: {
        type: String,
        default: ''
      },
      url_del: {
        type: String,
        default: ''
      },
      method: {
        type: String,
        default: 'get'
      },
      query: {
        type: Object,
        default: () => ({})
      },
      selection: {
        type: Boolean,
        default: true
      },
      index: {
        type: Boolean,
        default: true
      },
      fetchMethod: {
        type: Function,
        default: null
      },
      deleteMethod: {
        type: Function,
        default: null
      },
      actionColumn: {
        type: Object,
        default: () => ({
          label: '操作',
          sortable: false,
          fixed: 'right',
          width: 150,
          buttons: [
            {
              label: '修改',
              type: 'text',
              auth: 'edit',
              act: 'edit'
            },
            {
              label: '删除',
              type: 'text',
              auth: 'del',
              act: 'del'
            }
          ]
        })
      }
    }),
    data() {
      return {
        innerHeight: window.innerHeight - 232,
        dialogVisible: false,
        dialogImageUrl: '',
        listLoading: false,
        list: [],
        total: 0,
        page: 1,
        limit: 10,
        selectedRows: [],
        selectedIndex: []
      }
    },
    computed: {
      dataRender() {
        const { url, list, data } = this
        return url || this.fetchMethod ? list : data
      },
      columnsRender() {
        const selectionCol = { type: 'selection', sortable: false, align: 'center', width: 40 }
        const indexCol = { type: 'index', label: '序号', align: 'center', width: 50 }
        const { columns, selection, index } = this
        const cols = [...columns]
        if (index) cols.unshift(indexCol)
        if (selection) cols.unshift(selectionCol)
        cols.push(this.actionColumn)
        return cols
      }
    },
    watch: {
      dataRender() {
        if (!this.url && !this.fetchMethod) {
          this.total = this.dataRender.length
        }
      }
    },
    created() {
      for (const key in Table.methods) {
        if (!this[key]) {
          this[key] = (...args) => {
            this.$refs.table[key](...args)
          }
        }
      }
      this.resize()
      if (this.url || this.fetchMethod) {
        this.initLoad && this.search()
      } else {
        this.total = this.dataRender.length
      }
    },
    mounted() {
      window.addEventListener('resize', this.resize)
    },
    beforeDestroy() {
      window.removeEventListener('resize', this.resize)
    },
    methods: {
      async search(opt = {}) {
        this.selectedRows = []
        this.page = opt.page || 1
        const { query, url, method, page, limit, fetchMethod } = this

        if (!url && !fetchMethod) return

        const params = Object.assign({ page, limit }, query, opt)
        this.opt = params

        try {
          let res
          if (fetchMethod) {
            res = await fetchMethod(params)
          } else {
            const data = method === 'get' ? { params } : params
            res = await this.$http[method](url, data)
          }

          this.total = res.data.total
          this.list = res.data.records
        } catch (error) {
          this.localLoading = false
        }
      },
      refresh(opt = {}) {
        this.search({ ...this.opt, ...opt })
      },
      act(key, row, index) {
        this[key](row, index)
      },
      edit(row, index) {
        this.$emit('open', row, index)
      },
      async del({ id }, index) {
        try {
          await this.$confirm('此操作将永久删除该记录, 是否继续?', '警告', { type: 'warning' })

          if (this.deleteMethod) {
            await this.deleteMethod(id)
            this.refresh()
          } else if (this.url_del) {
            await this.$http.post(this.url_del, { id })
            this.refresh()
          } else {
            if (Array.isArray(index)) {
              for (let i = index.length - 1; i >= 0; i--) {
                this.data.splice(index[i], 1)
                this.selectedRows.splice(index[i], 1)
              }
            } else {
              this.data.splice(index, 1)
              this.selectedRows.splice(index, 1)
            }
          }
        } catch (error) {
          console.log('取消删除', error)
        }
      },
      selectionChange(rows) {
        this.selectedRows = rows
        const index = rows.map(o => this.dataRender.indexOf(o)).filter(i => i > -1).sort((a, b) => a - b)
        this.selectedIndex = index
      },
      rowClick(row) {
        this.toggleRowSelection(row)
      },
      handlePictureCardPreview(e) {
        this.dialogVisible = true
        this.dialogImageUrl = e.target.src
      },
      resize() {
        this.innerHeight = window.innerHeight - 232
      }
    }
  }
</script>

<style scoped>
</style>
