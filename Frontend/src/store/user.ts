localStorage.clear();

import { defineStore } from "pinia";
import { ref } from "vue";

import type { LoginResponse } from "@/types/user";

export const useUserStore = defineStore("user", () => {
  const user = ref<LoginResponse>();
  const set = (data: LoginResponse) => {
    user.value = data;
  };

  return {
    user,
    set,
  };
});
