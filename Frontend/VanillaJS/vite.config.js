import { resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { defineConfig } from 'vite';

const root = fileURLToPath(new URL('.', import.meta.url));

export default defineConfig({
  server: {
    port: 5173,
    strictPort: true
  },
  build: {
    rollupOptions: {
      input: {
        main: resolve(root, 'index.html'),
        create: resolve(root, 'create.html'),
        edit: resolve(root, 'edit.html'),
        delete: resolve(root, 'delete.html')
      }
    }
  }
});
