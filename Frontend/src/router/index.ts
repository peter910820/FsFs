import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

import { authStore } from "@/store/auth";
import { toastStore } from "@/store/toast";

import axios from "axios";

import type { ResponseType } from "@/types/response";

import type { RouteLocationNormalized, NavigationGuardNext } from "vue-router";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: () => import("@/Pages/MainPage.vue"),
  },
  {
    path: "/folder",
    name: "folder",
    component: () => import("@/Pages/FolderPage.vue"),
  },
  {
    path: "/upload",
    name: "upload",
    component: () => import("@/Pages/UploadPage.vue"),
    beforeEnter: async (to, from, next) => middlware(to, from, next),
  },
  // match all route
  {
    path: "/:pathMatch(.*)*",
    name: "notFound",
    component: () => import("@/Pages/NotFoundPage.vue"),
    meta: { layout: "empty" },
  },
];

const middlware = async (_to: RouteLocationNormalized, _from: RouteLocationNormalized, next: NavigationGuardNext) => {
  try {
    const apiUrl = import.meta.env.VITE_API_DOMAIN ? `${import.meta.env.VITE_API_DOMAIN}/api/auth` : "/api/auth";
    const response = await axios.post<ResponseType<null>>(
      apiUrl,
      {},
      {
        withCredentials: true,
      },
    );
    sessionStorage.setItem("msg", response.data.msg); // ?
    authStore.setStatus(true);
    next();
  } catch (error) {
    if (axios.isAxiosError(error)) {
      sessionStorage.setItem("msg", `${error.response?.status}: ${error.response?.data.msg}`);
      authStore.logout();
      toastStore.show("使用者尚未登入！");
      next("/folder");
    } else {
      authStore.logout();
      toastStore.show("發生未預期錯誤，已返回首頁", "error");
      next("/");
    }
  }
};

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
