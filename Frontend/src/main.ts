import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

import "materialize-css/dist/css/materialize.min.css";
import "materialize-css/dist/js/materialize.min.js";

// global stylesheets
import "./assets/styles/floating.css";
import "./assets/styles/button.css";

import { createPinia } from "pinia";

createApp(App).use(router).use(createPinia()).mount("#app");
