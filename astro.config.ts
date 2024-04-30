import { defineConfig } from 'astro/config'
import glsl from 'vite-plugin-glsl'

// https://astro.build/config
export default defineConfig({
	scopedStyleStrategy: 'where',
	devToolbar: {
		enabled: false,
	},
	server: {
		host: true,
	},
	vite: {
		plugins: [glsl()],
		build: {
			assetsInlineLimit: 0,
			cssCodeSplit: false,
			rollupOptions: {
				output: {
					entryFileNames: 'assets/scripts/entry.js',
					chunkFileNames: 'assets/scripts/chunk_[hash].js',
					assetFileNames: 'assets/styles/entry.css',
				},
			},
		},
	},
})
