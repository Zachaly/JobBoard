import { App, Plugin } from "vue";

const resumePlugin: Plugin = {
    install: (app: App, options) => {
        app.config.globalProperties.$resume = (id: number) => `https://localhost:5001/api/job-offer-application/${id}/resume`
    }
}

export default resumePlugin