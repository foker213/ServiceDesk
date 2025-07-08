import { ElMessage } from 'element-plus'
import { useUserStoreWithOut } from '@/store/modules/user'
import { BadRequestError } from './types'
import ky from 'ky'

const api = ky.extend({
    hooks: {
        beforeRequest: [
            (request) => {
                request.headers.set('X-Requested-With', 'ky')
            }
        ],
        beforeError: [
            async (error) => {
                const { response } = error

                if (response.status == 400) {
                    const json = (await response.json()) as BadRequestError
                    ElMessage.error(json.detail)
                }

                if ([401, 403].includes(response.status)) {
                    const json = (await response.json()) as BadRequestError
                    ElMessage.error(json.detail)
                    const userStore = useUserStoreWithOut()
                    userStore.logout()
                }
                return error
            }
        ]
    }
})

export { api }
