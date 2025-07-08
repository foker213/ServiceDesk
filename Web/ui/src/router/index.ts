import { createRouter, createWebHashHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import type { App } from 'vue'
import { Layout, getParentLayout } from '@/utils/routerHelper'
import { useI18n } from '@/hooks/web/useI18n'
import { NO_RESET_WHITE_LIST } from '@/constants'

const { t } = useI18n()

export const constantRouterMap: AppRouteRecordRaw[] = [
    {
        path: '/',
        component: Layout,
        redirect: '/manage/users',
        name: 'Root',
        meta: {
            hidden: true
        }
    },
    {
        path: '/redirect',
        component: Layout,
        name: 'Redirect',
        children: [
            {
                path: '/redirect/:path(.*)',
                name: 'Redirect',
                component: () => import('@/views/Redirect/Redirect.vue'),
                meta: {}
            }
        ],
        meta: {
            hidden: true,
            noTagsView: true
        }
    },
    {
        path: '/login',
        component: () => import('@/views/Login/Login.vue'),
        name: 'Login',
        meta: {
            hidden: true,
            title: t('router.login'),
            noTagsView: true
        }
    },
    {
        path: '/personal',
        component: Layout,
        redirect: '/personal/personal-center',
        name: 'Personal',
        meta: {
            title: t('router.personal'),
            hidden: true,
            canTo: true
        },
        children: [
            {
                path: 'personal-center',
                component: () => import('@/views/Personal/PersonalCenter/PersonalCenter.vue'),
                name: 'PersonalCenter',
                meta: {
                    title: t('router.personalCenter'),
                    hidden: true,
                    canTo: true
                }
            }
        ]
    },
    {
        path: '/404',
        component: () => import('@/views/Error/404.vue'),
        name: 'NotFound',
        meta: {
            hidden: true,
            title: '404',
            noTagsView: true
        }
    }
]

export const asyncRouterMap: AppRouteRecordRaw[] = [
    {
        name: 'Manage',
        path: '/manage',
        component: Layout,
        redirect: '/manage/users',
        meta: {
            title: t('router.manage'),
            icon: 'vi-bx:bxs-component',
            alwaysShow: true
        },
        children: [
            {
                name: 'Users',
                path: 'users',
                component: () => import('@/views/Manage/Users/Users.vue'),
                meta: {
                    title: t('router.users'),
                    noCache: true
                }
            }
        ]
    },
    {
        name: 'Catalog',
        path: '/catalog',
        component: Layout,
        redirect: '/catalog/myapplications',
        meta: {
            title: t('router.catalog'),
            icon: 'vi-bx:bxs-component',
            alwaysShow: true
        },
        children: [
            {
                name: 'MyApplications',
                path: 'myapplications',
                component: () => import('@/views/Catalog/MyApplications/MyApplications.vue'),
                meta: {
                    title: t('router.myApplications'),
                    noCache: true
                }
            },
            {
                name: 'UndistributedApplications',
                path: 'undistributedapplications',
                component: () => import('@/views/Catalog/UndistributedApplications/UndistributedApplications.vue'),
                meta: {
                    title: t('router.undistributedApplications'),
                    noCache: true
                }
            },
            {
                name: 'Archive',
                path: 'archive',
                component: () => import('@/views/Catalog/Archive/Archive.vue'),
                meta: {
                    title: t('router.archive'),
                    noCache: true
                }
            }
        ]
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    strict: true,
    routes: constantRouterMap as RouteRecordRaw[],
    scrollBehavior: () => ({ left: 0, top: 0 })
})

export const resetRouter = (): void => {
    router.getRoutes().forEach((route) => {
        const { name } = route
        if (name && !NO_RESET_WHITE_LIST.includes(name as string)) {
            router.hasRoute(name) && router.removeRoute(name)
        }
    })
}

export const setupRouter = (app: App<Element>) => {
    app.use(router)
}

export default router
