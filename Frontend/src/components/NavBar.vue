<script setup lang="ts">
import { computed, ref } from "vue";

import LoginDialog from "@/components/LoginDialog.vue";
import { useLoginStore } from "@/store/login";
import { themeStore } from "@/store/theme";
import { useUserStore } from "@/store/user";

const loginStore = useLoginStore();
const userStore = useUserStore();

const drawer = ref(false);
const loginOpen = ref(false);

const isDark = computed(() => themeStore.current.value === "dark");
const user = computed(() => userStore.user);
const displayName = computed(() => {
  if (!user.value) return "";
  return user.value.isAdmin ? `${user.value.username}/管理員` : user.value.username;
});

const toggleTheme = (event: MouseEvent) => {
  themeStore.toggle();
  const target = event.currentTarget;
  if (target instanceof HTMLElement) {
    target.blur();
  }
};
</script>

<template>
  <v-app-bar class="px-2 px-md-4" color="surface" elevation="0" flat border height="72">
    <v-app-bar-nav-icon class="d-md-none me-2" rounded="lg" @click="drawer = !drawer" />

    <v-btn class="me-2" icon rounded="xl" variant="text" to="/" aria-label="首頁">
      <v-icon icon="mdi-home" size="26" />
    </v-btn>

    <v-spacer />

    <div class="d-none d-md-flex ga-4 align-center me-4">
      <v-btn rounded="xl" variant="text" to="/" prepend-icon="mdi-home">首頁</v-btn>
      <v-btn rounded="xl" variant="text" to="/folder" prepend-icon="mdi-folder-open">檔案夾</v-btn>
    </div>

    <div class="d-flex align-center ga-3 me-1 me-md-2">
      <v-btn
        icon
        rounded="xl"
        variant="text"
        :aria-label="isDark ? '切換淺色主題' : '切換深色主題'"
        @click="toggleTheme"
      >
        <v-icon :icon="isDark ? 'mdi-white-balance-sunny' : 'mdi-weather-night'" />
      </v-btn>

      <template v-if="loginStore.status && user">
        <div class="d-flex align-center ga-3 user-chip px-3 py-1">
          <v-avatar size="40" color="primary" rounded="circle">
            <v-img v-if="user.avatar" :src="user.avatar" :alt="user.username" />
            <v-icon v-else icon="mdi-account" />
          </v-avatar>
          <span class="text-body-2 font-weight-medium d-none d-sm-inline">{{ displayName }}</span>
        </div>
      </template>
      <v-btn v-else rounded="xl" color="primary" variant="flat" prepend-icon="mdi-key" @click="loginOpen = true">
        登入
      </v-btn>
    </div>
  </v-app-bar>

  <v-navigation-drawer v-model="drawer" temporary location="start" width="280">
    <v-list class="pa-3" nav>
      <v-list-item class="mb-2" rounded="xl" title="首頁" prepend-icon="mdi-home" to="/" @click="drawer = false" />
      <v-list-item
        class="mb-2"
        rounded="xl"
        title="檔案夾"
        prepend-icon="mdi-folder-open"
        to="/folder"
        @click="drawer = false"
      />
      <v-list-item
        v-if="!loginStore.status"
        class="mb-2"
        rounded="xl"
        title="登入"
        prepend-icon="mdi-key"
        @click="
          drawer = false;
          loginOpen = true;
        "
      />
      <v-list-item v-else class="mb-2" rounded="xl" :title="displayName || '已登入'" prepend-icon="mdi-account" />
    </v-list>
  </v-navigation-drawer>

  <LoginDialog v-model="loginOpen" />
</template>

<style scoped>
.user-chip {
  border-radius: 999px;
  background: rgba(var(--v-theme-surface-light), 0.65);
}
</style>
