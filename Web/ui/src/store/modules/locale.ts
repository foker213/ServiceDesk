import { defineStore } from 'pinia'
import { store } from '../index'
import en from 'element-plus/es/locale/lang/en'
import ru from 'element-plus/es/locale/lang/ru'
import { useStorage } from '@/hooks/web/useStorage'
import { LocaleDropdownType } from '@/components/LocaleDropdown'

const { getStorage, setStorage } = useStorage('localStorage')

const elLocaleMap = {
    en: en,
    ru: ru
}
interface LocaleState {
    currentLocale: LocaleDropdownType
    localeMap: LocaleDropdownType[]
}

export const useLocaleStore = defineStore('locales', {
    state: (): LocaleState => {
        return {
            currentLocale: {
                lang: getStorage('lang') || 'ru',
                elLocale: elLocaleMap[getStorage('lang') || 'ru']
            },
            localeMap: [
                {
                    lang: 'ru',
                    name: 'Русский'
                },
                {
                    lang: 'en',
                    name: 'English'
                }
            ]
        }
    },
    getters: {
        getCurrentLocale(): LocaleDropdownType {
            return this.currentLocale
        },
        getLocaleMap(): LocaleDropdownType[] {
            return this.localeMap
        }
    },
    actions: {
        setCurrentLocale(localeMap: LocaleDropdownType) {
            // this.locale = Object.assign(this.locale, localeMap)
            this.currentLocale.lang = localeMap?.lang
            this.currentLocale.elLocale = elLocaleMap[localeMap?.lang]
            setStorage('lang', localeMap?.lang)
        }
    }
})

export const useLocaleStoreWithOut = () => {
    return useLocaleStore(store)
}
