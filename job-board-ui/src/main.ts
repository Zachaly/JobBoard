import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import axios from "axios";
import { createPinia } from "pinia";
import datePlugin from "./plugins/date-plugin";
import imagePlugin from "./plugins/image-plugin";
import QueryString from "qs";
import resumePlugin from "./plugins/resume-plugin";
import "@vuepic/vue-datepicker/dist/main.css";
import VueDatePicker from '@vuepic/vue-datepicker';


axios.defaults.baseURL = "https://localhost:5001/api/";
axios.defaults.paramsSerializer = (params: any) =>
  QueryString.stringify(params);

const pinia = createPinia();

const app = createApp(App)
  .use(pinia)
  .use(router)
  .use(datePlugin)
  .use(imagePlugin)
  .use(resumePlugin);

app.component("VueDatePicker", VueDatePicker);
app.mount("#app");
