<script setup lang="ts">
import { ref, watch, computed, onMounted, unref } from 'vue'
import { useAppStore } from '@/store/modules/app'
import { useDesign } from '@/hooks/web/useDesign'

const { getPrefixCls } = useDesign()

const prefixCls = getPrefixCls('logo')

const appStore = useAppStore()

const show = ref(true)

const title = computed(() => appStore.getTitle)

const layout = computed(() => appStore.getLayout)

const isDark = computed(() => appStore.getIsDark)

const collapse = computed(() => appStore.getCollapse)

onMounted(() => {
    if (unref(collapse)) show.value = false
})

watch(
    () => collapse.value,
    (collapse: boolean) => {
        if (unref(layout) === 'topLeft' || unref(layout) === 'cutMenu') {
            show.value = true
            return
        }
        show.value = !collapse
    }
)

watch(
    () => layout.value,
    (layout) => {
        if (layout === 'top' || layout === 'cutMenu') {
            show.value = true
        } else {
            if (unref(collapse)) {
                show.value = false
            } else {
                show.value = true
            }
        }
    }
)
</script>

<template>
    <div>
        <router-link
            :class="[
                prefixCls,
                layout !== 'classic' ? `${prefixCls}__Top` : '',
                'flex !h-[var(--logo-height)] items-center cursor-pointer pl-8px relative decoration-none overflow-hidden'
            ]"
            to="/"
        >
            <div>
                <img
                    v-if="isDark === false && collapse"
                    src="@/assets/svgs/logo-white-small.svg"
                    class="h-50px"
                    style="transform: translate(-30px, 2px)"
                    text=""
                />
                <img
                    v-if="isDark === false && !collapse"
                    src="@/assets/svgs/logo-white.svg"
                    class="h-50px"
                    style="transform: translate(30px, 0px)"
                />
            </div>

            <div>
                <img
                    v-if="isDark === true && collapse"
                    src="@/assets/svgs/logo-dark-small.svg"
                    class="h-50px"
                    style="transform: translate(-30px, 2px)"
                    text=""
                />
                <img
                    v-if="isDark === true && !collapse"
                    src="@/assets/svgs/logo-dark.svg"
                    class="h-50px"
                    style="transform: translate(30px, 0px)"
                />
            </div>
        </router-link>
    </div>
</template>
