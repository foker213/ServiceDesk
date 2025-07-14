import { SUCCESS_CODE } from '@/constants'

const timeout = 1000

const adminList = [
    {
        name: 'Manage',
        path: '/manage',
        component: '#',
        redirect: '/manage/users',
        meta: {
            title: 'router.manage',
            icon: 'vi-bx:bxs-component',
            alwaysShow: true
        },
        children: [
            {
                name: 'Users',
                path: 'users',
                component: 'views/Manage/Users/Users',
                meta: {
                    title: 'router.users',
                }
            }
        ]
    },
    {
        name: 'Catalog',
        path: '/catalog',
        component: '#',
        redirect: '/catalog/myrequests',
        meta: {
            title: 'router.catalog',
            icon: 'vi-bx:bxs-component',
            alwaysShow: true
        },
        children: [
            {
                name: 'MyRequests',
                path: 'myrequests',
                component: 'views/Catalog/MyRequests/MyRequests',
                meta: {
                    title: 'router.myRequests',
                    noCache: true,
                }
            },
            {
                name: 'UndistributedRequests',
                path: 'undistributedrequests',
                component: 'views/Catalog/UndistributedRequests/UndistributedRequests',
                meta: {
                    title: 'router.undistributedRequests',
                    noCache: true,
                }
            },
            {
                name: 'Archive',
                path: 'archive',
                component: 'views/Catalog/Archive/Archive',
                meta: {
                    title: 'router.archive',
                    noCache: true,
                }
            }
        ]
    }
]

export default [
    {
        url: '/mock/role/list',
        method: 'get',
        timeout,
        response: () => {
            return {
                code: SUCCESS_CODE,
                data: adminList
            }
        }
    }
]
