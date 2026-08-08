import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import vuetify from "vite-plugin-vuetify";

// https://vite.dev/config/
export default defineConfig({
  plugins: [vue(), vuetify({ autoImport: true })],
  resolve: {
    tsconfigPaths: true,
    alias: {
      "@": "/src",
    },
  },
  server: {
    host: "127.0.0.1",
  },
  build: {
    rollupOptions: {
      output: {
        // 檔名完全使用hash
        entryFileNames: "assets/[hash].js",
        chunkFileNames: "assets/[hash].js",
        assetFileNames: "assets/[hash].[ext]",
      },
    },
  },
});
