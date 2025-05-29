import { defineConfig } from 'vite'
import { createVuePlugin } from 'vite-plugin-vue2'
import { resolve } from 'path'
import { svgBuilder } from './src/plugins/svgBuilder'
// vite.config.js
export default defineConfig({
  base: './',
  esbuild: {
    jsxFactory: 'h',
    jsxFragment: 'Fragment'
  },
  plugins: [
    svgBuilder('./src/icons/svg/'),
    createVuePlugin({
      jsx: true, vueTemplateOptions: { compilerOptions: { whitespace: 'condense' }},
      jsxOptions: { compositionAPI: true }
    })
  ],
  resolve: {
    extensions: ['.vue', '.mjs', '.js', '.ts', '.jsx', '.tsx', '.json'],
    alias: {
      // vue2项目别名一般都是@，vue3中一般使用/@/, 为方便使用
      '@': resolve('src')
    }
  },
  server: {
    host: true,          // Доступ по 0.0.0.0 (нужно для Docker)
    port: 3000,          // Явно указываем порт (опционально)
    watch: {
      usePolling: true,  // Обязательно для Hot Reload в Docker
      interval: 1000     // Проверка изменений каждую секунду
    }
  }
})
