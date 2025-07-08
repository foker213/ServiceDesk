import { useI18n } from '@/hooks/web/useI18n'
import { FormItemRule } from 'element-plus'

const { t } = useI18n()

interface LengthRange {
    min: number
    max: number
    message?: string
}

export const useValidator = () => {
    const required = (message?: string): FormItemRule => {
        return {
            required: true,
            message: message || t('common.required')
        }
    }

    const parseArrayNumbers = (delimiter: string): FormItemRule => {
        return {
            validator: (_, val, callback) => {
                const valStr = val.toString()
                if (valStr !== '') {
                    if (isNotCorrectCharacters(valStr)) {
                        callback(new Error(t('error.valueNotNumber')))
                    }

                    const numsStr: string[] = valStr.split(/[,;]+/)
                    if (Array.isArray(numsStr) && numsStr.length) {
                        numsStr.forEach((el) => {
                            const num: number = parseInt(el, 10)
                            if (Number.isNaN(num)) {
                                callback(new Error(t('error.valueNotNumber')))
                            }
                        })
                    }
                }

                callback()
            }
        }
    }

    function isNotCorrectCharacters(str) {
        return /^[\s0-9,;]+$/.test(str) == false // spaces, digits, and symbols [;,]
    }

    const lengthRange = (options: LengthRange): FormItemRule => {
        const { min, max, message } = options

        return {
            min,
            max,
            message: message || t('common.lengthRange', { min, max })
        }
    }

    const notSpace = (message?: string): FormItemRule => {
        return {
            validator: (_, val, callback) => {
                if (val?.indexOf(' ') !== -1) {
                    callback(new Error(message || t('common.notSpace')))
                } else {
                    callback()
                }
            }
        }
    }

    const notSpecialCharacters = (message?: string): FormItemRule => {
        return {
            validator: (_, val, callback) => {
                if (/[`~!@#$%^&*()_+<>?:"{},.\/;'[\]]/gi.test(val)) {
                    callback(new Error(message || t('common.notSpecialCharacters')))
                } else {
                    callback()
                }
            }
        }
    }

    const phone = (message?: string): FormItemRule => {
        return {
            validator: (_, val, callback) => {
                if (!val) return callback()
                if (!/^1[3456789]\d{9}$/.test(val)) {
                    callback(new Error(message || '请输入正确的手机号码'))
                } else {
                    callback()
                }
            }
        }
    }

    const email = (message?: string): FormItemRule => {
        return {
            validator: (_, val, callback) => {
                if (!val) return callback()
                if (!/^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/.test(val)) {
                    callback(new Error(message || '请输入正确的邮箱'))
                } else {
                    callback()
                }
            }
        }
    }

    const maxlength = (max: number, message?: string): FormItemRule => {
        return {
            max,
            message: message || 'Поле должно содержать не более ' + max + ' знаков'
        }
    }

    const minlength = (min: number, message?: string): FormItemRule => {
        return {
            min,
            message: message || 'Поле должно содержать не менее ' + min + ' знаков'
        }
    }

    const check = (message?: string): FormItemRule => {
        return {
            validator: (_, val, callback) => {
                if (!val) {
                    callback(new Error(message || t('common.required')))
                } else {
                    callback()
                }
            }
        }
    }

    return {
        required,
        parseArrayNumbers,
        lengthRange,
        notSpace,
        notSpecialCharacters,
        phone,
        email,
        maxlength,
        minlength,
        check
    }
}
