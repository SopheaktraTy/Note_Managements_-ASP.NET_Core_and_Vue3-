// vite.config.ts
import { defineConfig } from "vite"
import vue from "@vitejs/plugin-vue"
import { fileURLToPath, URL } from "node:url"

export default defineConfig({
  plugins: [vue()],
  resolve: { alias: { "@": fileURLToPath(new URL("./src", import.meta.url)) } },
  server: {
    proxy: {
      "/api": {
        target: "http://localhost:5156", // use http or https to match your API; your JWT Issuer shows http
        changeOrigin: true,
        secure: false
      }
    }
  },
  build: { outDir: "wwwroot", emptyOutDir: true }
})
