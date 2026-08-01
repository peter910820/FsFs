<script setup lang="ts">
import axios from "axios";
import { ref, watch } from "vue";
import { useRouter } from "vue-router";

import { useLoginStore } from "@/store/login";
import { useUserStore } from "@/store/user";
import type { ResponseType } from "@/types/response";
import type { LoginResponse } from "@/types/user";

const open = defineModel<boolean>({ default: false });

const router = useRouter();
const loginStore = useLoginStore();
const userStore = useUserStore();

const loading = ref(false);
const form = ref({
  username: "",
  password: "",
});

watch(open, (value) => {
  if (!value) {
    form.value = { username: "", password: "" };
  }
});

const close = () => {
  open.value = false;
};

const handleSubmit = async () => {
  if (!form.value.username.trim() || !form.value.password) return;

  loading.value = true;
  try {
    const apiUrl = import.meta.env.VITE_API_DOMAIN ? `${import.meta.env.VITE_API_DOMAIN}/api/login` : "/api/login";
    const response = await axios.post<ResponseType<LoginResponse>>(apiUrl, form.value, {
      withCredentials: true,
    });
    sessionStorage.setItem("errorMsg", response.data.msg);
    if (response.data.data) {
      userStore.set(response.data.data);
      loginStore.set(true);
      close();
    }
    router.push("/");
  } catch (error) {
    if (axios.isAxiosError(error)) {
      sessionStorage.setItem("errorMsg", `${error.response?.status}: ${error.response?.data.msg}`);
    } else {
      sessionStorage.setItem("errorMsg", String(error));
    }
    router.push("/error");
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <v-dialog v-model="open" max-width="460" scrim="black">
    <v-card class="login-card overflow-hidden" rounded="xl" elevation="8">
      <div class="login-hero pa-8 text-center">
        <v-avatar class="mb-4" color="primary" size="64" variant="tonal">
          <v-icon icon="mdi-shield-key" size="32" />
        </v-avatar>
        <div class="text-h5 font-weight-bold mb-1">歡迎回來</div>
        <div class="text-body-2 text-medium-emphasis">登入後即可上傳與管理檔案</div>
      </div>

      <v-card-text class="px-8 pb-2">
        <v-form @submit.prevent="handleSubmit">
          <v-text-field
            v-model="form.username"
            class="mb-4"
            label="使用者名稱"
            placeholder="username"
            prepend-inner-icon="mdi-account-circle-outline"
            autocomplete="username"
            hide-details="auto"
            required
          />
          <v-text-field
            v-model="form.password"
            label="密碼"
            placeholder="password"
            type="password"
            prepend-inner-icon="mdi-lock-outline"
            autocomplete="current-password"
            hide-details="auto"
            required
          />
        </v-form>
      </v-card-text>

      <v-card-actions class="px-8 pt-4 pb-8 ga-3 flex-column align-stretch">
        <v-btn
          block
          rounded="xl"
          color="primary"
          variant="flat"
          size="large"
          :loading="loading"
          prepend-icon="mdi-login"
          @click="handleSubmit"
        >
          登入
        </v-btn>
        <v-btn block rounded="xl" variant="text" @click="close">取消</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<style scoped>
.login-hero {
  background: linear-gradient(145deg, rgba(var(--v-theme-primary), 0.16) 0%, rgba(var(--v-theme-surface), 1) 70%);
}
</style>
