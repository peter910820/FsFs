<script setup lang="ts">
import { onMounted, ref } from "vue";

import BlockError from "@/components/BlockError.vue";
import { getFile } from "@/utils/apiHandler";
import { fileIcon, openStaticFile } from "@/utils/files";

const files = ref<string[]>([]);
const loading = ref(true);
const error = ref<string | null>(null);

const loadFiles = async () => {
  loading.value = true;
  error.value = null;
  const response = await getFile();
  if (response && response.status === 200) {
    files.value = response.data.data ?? [];
  } else {
    files.value = [];
    error.value = response?.data?.msg ?? "無法取得檔案列表";
  }
  loading.value = false;
};

onMounted(loadFiles);
</script>

<template>
  <v-row justify="center">
    <v-col cols="12" md="10" lg="8">
      <div class="home-hero pa-8 mb-6 rounded-xl">
        <div class="text-h4 font-weight-bold mb-2">FsFs</div>
        <p class="text-body-1 text-medium-emphasis mb-0">
          個人資源伺服器：瀏覽資料夾、下載檔案，登入後即可上傳與管理內容。
        </p>
      </div>

      <div class="d-flex align-center justify-space-between mb-4">
        <div>
          <div class="text-h6 font-weight-bold">最近上傳</div>
        </div>
        <v-btn rounded="xl" variant="tonal" color="primary" to="/folder" prepend-icon="mdi-folder-outline">
          瀏覽全部
        </v-btn>
      </div>

      <v-card rounded="xl" elevation="2">
        <v-skeleton-loader v-if="loading" type="list-item-avatar@5" />
        <BlockError v-else-if="error" :message="error" @retry="loadFiles" />
        <v-list v-else-if="files.length" lines="one" class="py-2">
          <v-list-item
            v-for="(item, index) in files"
            :key="index"
            :prepend-icon="fileIcon(item)"
            :title="item"
            rounded="lg"
            class="mx-2"
            @click="openStaticFile(item)"
          >
            <template #append>
              <v-icon icon="mdi-open-in-new" size="small" class="text-medium-emphasis" />
            </template>
          </v-list-item>
        </v-list>
        <div v-else class="pa-10 text-center text-medium-emphasis">
          <v-icon icon="mdi-file-search-outline" size="48" class="mb-3" />
          <div>目前尚無檔案</div>
        </div>
      </v-card>
    </v-col>
  </v-row>
</template>

<style scoped>
.home-hero {
  background: linear-gradient(135deg, rgba(var(--v-theme-primary), 0.14), rgba(var(--v-theme-accent), 0.08));
  border: 1px solid rgba(var(--v-border-color), var(--v-border-opacity));
}
</style>
