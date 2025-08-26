import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

import MainView from "../views/MainView.vue";
import Cookies from "js-cookie";

import { getDataEntryPoint } from "@/router/guard";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "main",
    component: MainView,
    children: [
      {
        path: "",
        name: "home",
        component: () => import("@/components/MainPage.vue"),
        beforeEnter: (_to, _from, next) => {
          if (Cookies.get("session_id") !== undefined) {
            next();
          } else {
            next("/login");
          }
        },
      },
      {
        path: "/login",
        name: "login",
        component: () => import("@/components/LoginPage.vue"),
      },
      {
        path: "/folder",
        name: "folder",
        component: () => import("@/components/FolderPage.vue"),
        beforeEnter: async (to, from, next) => getDataEntryPoint(to, from, next),
      },
      {
        path: "/markdown-writer",
        name: "markdown-writer",
        component: () => import("@/components/MdWriter.vue"),
      },
      {
        path: "/error",
        name: "error",
        component: () => import("@/components/ErrorPage.vue"),
      },
      // match all route
      {
        path: ":pathMatch(.*)*",
        name: "notFound",
        component: () => import("@/components/ErrorPage.vue"),
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
