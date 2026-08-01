import { readonly, ref } from "vue";

import type { LoginResponse } from "@/types/user";

const status = ref(false);
const user = ref<LoginResponse | null>(null);

const setStatus = (value: boolean) => {
  status.value = value;
  if (!value) {
    user.value = null;
  }
};

const setUser = (data: LoginResponse) => {
  user.value = data;
};

const login = (data: LoginResponse) => {
  user.value = data;
  status.value = true;
};

const logout = () => {
  status.value = false;
  user.value = null;
};

export const authStore = {
  status: readonly(status),
  user: readonly(user),
  setStatus,
  setUser,
  login,
  logout,
};
