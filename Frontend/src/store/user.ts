localStorage.clear();

import { defineStore } from "pinia";
import { ref } from "vue";

import type { UserType } from "@/types/user";

export const useUserStore = defineStore("user", () => {
  const user = ref<UserType>();
  const set = (data: UserType) => {
    user.value = data;
  };

  return {
    user,
    set,
  };
});
