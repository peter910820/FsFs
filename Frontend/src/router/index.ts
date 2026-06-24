import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

import { useLoginStore } from "@/store/login";

import axios from "axios";

import type { ResponseType } from "@/types/response";

import type { RouteLocationNormalized, NavigationGuardNext } from "vue-router";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: () => import("@/components/MainPage.vue"),
    beforeEnter: async (to, from, next) => middlware(to, from, next),
  },
  {
    path: "/folder",
    name: "folder",
    component: () => import("@/components/FolderPage.vue"),
  },
  {
    path: "/error",
    name: "error",
    component: () => import("@/components/ErrorPage.vue"),
  },
  // match all route
  {
    path: "/:pathMatch(.*)*",
    name: "notFound",
    component: () => import("@/components/NotFoundPage.vue"),
    meta: { layout: "empty" },
  },
];

const middlware = async (_to: RouteLocationNormalized, _from: RouteLocationNormalized, next: NavigationGuardNext) => {
  const loginStore = useLoginStore();
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
    loginStore.set(true);
    next();
  } catch (error) {
    if (axios.isAxiosError(error)) {
      sessionStorage.setItem("msg", `${error.response?.status}: ${error.response?.data.msg}`);
      loginStore.set(false);
      M.toast({ html: "使用者尚未登入！" });
      router.push("/folder");
    } else {
      sessionStorage.setItem("msg", String(error));
      router.push("/error");
    }
  }
};

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
