import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import vuetify from "./plugins/vuetify";

import "materialize-css/dist/css/materialize.min.css";
import "materialize-css/dist/js/materialize.min.js";

import { createPinia } from "pinia";

createApp(App).use(router).use(createPinia()).use(vuetify).mount("#app");
