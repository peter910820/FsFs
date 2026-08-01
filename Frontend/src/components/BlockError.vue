<script setup lang="ts">
withDefaults(
  defineProps<{
    message?: string;
  }>(),
  {
    message: "載入失敗",
  },
);

defineEmits<{
  retry: [];
}>();
</script>

<template>
  <div class="pa-10 text-center">
    <v-icon icon="mdi-alert-circle-outline" size="48" color="error" class="mb-3" />
    <div class="text-body-1 font-weight-medium mb-1">載入失敗</div>
    <div class="text-body-2 text-medium-emphasis mb-5">{{ message }}</div>
    <v-btn rounded="xl" color="primary" variant="tonal" prepend-icon="mdi-refresh" @click="$emit('retry')">重試</v-btn>
  </div>
</template>
