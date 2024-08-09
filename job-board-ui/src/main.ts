import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import axios from "axios";
import { createPinia } from "pinia";
import datePlugin from "./plugins/date-plugin";
import imagePlugin from "./plugins/image-plugin";
import QueryString from "qs";

axios.defaults.baseURL = "https://localhost:5001/api/";
axios.defaults.paramsSerializer = (params: any) => QueryString.stringify(params);

const pinia = createPinia();

createApp(App)
  .use(pinia)
  .use(router)
  .use(datePlugin)
  .use(imagePlugin)
  .mount("#app");
