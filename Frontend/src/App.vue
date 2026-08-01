<template>
  <v-app>
    <component :is="layoutComponent" />
    <v-snackbar v-model="visible" :color="color" rounded="pill" timeout="3000">
      {{ text }}
    </v-snackbar>
  </v-app>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "vue-router";
import { useTheme } from "vuetify";

import DefaultLayout from "@/layouts/DefaultLayout.vue";
import EmptyLayout from "@/layouts/EmptyLayout.vue";
import { themeStore } from "@/store/theme";
import { toastStore } from "@/store/toast";

themeStore.bind(useTheme());

const { visible, text, color } = toastStore;

const route = useRoute();

const layoutComponent = computed(() => {
  if (route.meta.layout === "empty") return EmptyLayout;
  return DefaultLayout;
});
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  min-height: 100vh;
  margin-bottom: 0px !important;
}
</style>
