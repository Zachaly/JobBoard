import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import axios from "axios";
import { createPinia } from "pinia";
import datePlugin from "./plugins/date-plugin";

axios.defaults.baseURL = "https://localhost:5001/api/";

const pinia = createPinia();

createApp(App).use(pinia).use(router).use(datePlugin).mount("#app");
