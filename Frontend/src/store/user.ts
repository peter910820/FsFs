localStorage.clear();

import { defineStore } from "pinia";
import { ref } from "vue";

import type { UserInfo } from "@/types/user";

export const useUserStore = defineStore("user", () => {
  const user = ref<UserInfo>();
  const set = (data: UserInfo) => {
    user.value = data;
  };

  return {
    user,
    set,
  };
});
