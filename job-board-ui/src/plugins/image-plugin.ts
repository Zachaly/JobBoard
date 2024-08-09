import { App, Plugin } from "vue";

const imagePlugin: Plugin = {
    install: (app: App, options) => {
        app.config.globalProperties.$companyImage = (id: number) => `https://localhost:5001/api/company-account/${id}/image`
    }
}

export default imagePlugin