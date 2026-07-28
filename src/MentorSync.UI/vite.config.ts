import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import process from "process";

function parsePort(value: string | undefined): number | undefined {
    const port = Number(value);

    return Number.isInteger(port) && port > 0 && port <= 65535
        ? port
        : undefined;
}

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), "");
    const port = parsePort(env.PORT) ?? parsePort(env.VITE_PORT);

    return {
        plugins: [react(), tailwindcss()],
        server: {
            ...(port === undefined ? {} : { port }),
            proxy: {
                "/api": {
                    target:
                        process.env.services__api__https__0 ||
                        process.env.services__api__http__0,
                    changeOrigin: true,
                    rewrite: (path) => path.replace(/^\/api/, ""),
                    secure: false,
                },
            },
        },
        build: {
            outDir: "dist",
            rollupOptions: {
                input: "./index.html",
            },
        },
    };
});
