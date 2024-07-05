import { App, Plugin } from "vue";

const datePlugin: Plugin = {
    install: (app: App, options) => {
        app.config.globalProperties.$fromUtcDate = (dateString: string) => {
            const date = new Date(dateString)

            return date.toLocaleDateString()
        }
    }
}

export default datePlugin