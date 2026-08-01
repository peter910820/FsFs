<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import axios from "axios";
import type { ResponseType } from "@/types/response";

import BlockError from "@/components/BlockError.vue";
import { getDirectory, getFile } from "@/utils/apiHandler";
import { useLoginStore } from "@/store/login";

const loginStore = useLoginStore();

const directories = ref<string[]>([]);
const files = ref<string[]>([]);
const selectedDir = ref<string | null>(null);
const loadingDirs = ref(true);
const loadingFiles = ref(false);
const errorDirs = ref<string | null>(null);
const errorFiles = ref<string | null>(null);

const deleteDialog = ref(false);
const pendingDelete = ref<string | null>(null);
const deleting = ref(false);

const snackbar = ref(false);
const snackbarText = ref("");
const snackbarColor = ref("error");

const currentLabel = computed(() => (selectedDir.value ? selectedDir.value : "全部檔案"));

const apiBase = () => import.meta.env.VITE_API_DOMAIN || "";

const errorMessage = (error: unknown, fallback: string) => {
  if (axios.isAxiosError(error)) {
    return error.response?.data?.msg || error.message || fallback;
  }
  if (error instanceof Error) {
    return error.message || fallback;
  }
  return fallback;
};

const showSnackbar = (text: string, color = "error") => {
  snackbarText.value = text;
  snackbarColor.value = color;
  snackbar.value = true;
};

const fileIcon = (name: string) => {
  const lower = name.toLowerCase();
  if ([".png", ".jpg", ".jpeg", ".gif", ".webp"].some((ext) => lower.endsWith(ext))) return "mdi-image-outline";
  if ([".zip", ".rar", ".7z", ".tar.gz"].some((ext) => lower.endsWith(ext))) return "mdi-folder-zip-outline";
  if ([".go", ".py", ".fs", ".cs", ".ts", ".js"].some((ext) => lower.endsWith(ext))) return "mdi-code-braces";
  if ([".mp4", ".mkv", ".webm"].some((ext) => lower.endsWith(ext))) return "mdi-video-outline";
  if ([".mp3", ".wav", ".flac"].some((ext) => lower.endsWith(ext))) return "mdi-music-note";
  if (lower.endsWith(".xp3")) return "mdi-cog-outline";
  return "mdi-file-outline";
};

const loadDirectories = async () => {
  loadingDirs.value = true;
  errorDirs.value = null;
  const response = await getDirectory();
  if (response && response.status === 200) {
    directories.value = response.data.data ?? [];
  } else {
    directories.value = [];
    errorDirs.value = response?.data?.msg ?? "無法取得資料夾列表";
  }
  loadingDirs.value = false;
};

const loadAllFiles = async () => {
  loadingFiles.value = true;
  errorFiles.value = null;
  selectedDir.value = null;
  const response = await getFile();
  if (response && response.status === 200) {
    files.value = response.data.data ?? [];
  } else {
    files.value = [];
    errorFiles.value = response?.data?.msg ?? "無法取得檔案列表";
  }
  loadingFiles.value = false;
};

const expandDetails = async (folder: string) => {
  loadingFiles.value = true;
  errorFiles.value = null;
  selectedDir.value = folder;
  try {
    const response = await axios.get<ResponseType<string[]>>(`${apiBase()}/api/files?dir=${folder}`);
    files.value = response.data.data ?? [];
  } catch (error) {
    files.value = [];
    errorFiles.value = errorMessage(error, "無法取得檔案列表");
  } finally {
    loadingFiles.value = false;
  }
};

const openFile = (path: string) => {
  window.location.href = `${import.meta.env.VITE_STATIC_FILE_DOMAIN}/${path}`;
};

const askDelete = (path: string) => {
  pendingDelete.value = path;
  deleteDialog.value = true;
};

const confirmDelete = async () => {
  if (!pendingDelete.value) return;
  deleting.value = true;
  try {
    await axios.delete<ResponseType<string[]>>(`${apiBase()}/api/file`, {
      data: { fileName: pendingDelete.value },
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    });
    deleteDialog.value = false;
    pendingDelete.value = null;
    if (selectedDir.value) {
      await expandDetails(selectedDir.value);
    } else {
      await loadAllFiles();
    }
  } catch (error) {
    deleteDialog.value = false;
    showSnackbar(errorMessage(error, "刪除失敗"));
  } finally {
    deleting.value = false;
  }
};

onMounted(async () => {
  await Promise.all([loadDirectories(), loadAllFiles()]);
});
</script>

<template>
  <div>
    <div class="mb-6">
      <div class="text-h5 font-weight-bold">伺服器資源</div>
      <div class="text-body-2 text-medium-emphasis">目前檢視：{{ currentLabel }}</div>
    </div>

    <v-row>
      <!-- 手機：橫向資料夾 chip -->
      <v-col cols="12" class="d-md-none pb-0">
        <BlockError v-if="errorDirs && !loadingDirs" :message="errorDirs" @retry="loadDirectories" />
        <div v-else class="folder-chips d-flex ga-2 pb-2">
          <v-chip
            :color="selectedDir === null ? 'primary' : undefined"
            :variant="selectedDir === null ? 'flat' : 'tonal'"
            prepend-icon="mdi-file-multiple-outline"
            @click="loadAllFiles"
          >
            全部
          </v-chip>
          <v-chip
            v-for="dir in directories"
            :key="dir"
            :color="selectedDir === dir ? 'primary' : undefined"
            :variant="selectedDir === dir ? 'flat' : 'tonal'"
            prepend-icon="mdi-folder-outline"
            @click="expandDetails(dir)"
          >
            {{ dir }}
          </v-chip>
        </div>
      </v-col>

      <!-- 桌面：左欄資料夾 -->
      <v-col cols="12" md="3" class="d-none d-md-block">
        <v-card class="folder-panel" rounded="xl" elevation="2">
          <v-skeleton-loader v-if="loadingDirs" type="list-item@4" />
          <BlockError v-else-if="errorDirs" :message="errorDirs" @retry="loadDirectories" />
          <v-list v-else nav density="comfortable" class="pa-2" color="primary">
            <v-list-subheader>資料夾</v-list-subheader>
            <v-list-item
              title="全部"
              prepend-icon="mdi-file-multiple-outline"
              :active="selectedDir === null"
              active-class="folder-item--active"
              rounded="lg"
              @click="loadAllFiles"
            />
            <v-list-item
              v-for="dir in directories"
              :key="dir"
              :title="dir"
              prepend-icon="mdi-folder-outline"
              :active="selectedDir === dir"
              active-class="folder-item--active"
              rounded="lg"
              @click="expandDetails(dir)"
            />
          </v-list>
        </v-card>
      </v-col>

      <!-- 右欄檔案 -->
      <v-col cols="12" md="9">
        <v-card rounded="xl" elevation="2">
          <v-skeleton-loader v-if="loadingFiles" type="list-item-avatar@6" />
          <BlockError
            v-else-if="errorFiles"
            :message="errorFiles"
            @retry="selectedDir ? expandDetails(selectedDir) : loadAllFiles()"
          />
          <v-list v-else-if="files.length" lines="one" class="py-2">
            <v-list-item
              v-for="item in files"
              :key="item"
              :prepend-icon="fileIcon(item)"
              :title="item"
              rounded="lg"
              class="mx-2"
              @click="openFile(item)"
            >
              <template #append>
                <v-btn
                  v-if="loginStore.status"
                  icon="mdi-delete-outline"
                  size="small"
                  variant="text"
                  color="error"
                  aria-label="刪除"
                  @click.stop="askDelete(item)"
                />
                <v-icon v-else icon="mdi-open-in-new" size="small" class="text-medium-emphasis" />
              </template>
            </v-list-item>
          </v-list>
          <div v-else class="pa-10 text-center text-medium-emphasis">
            <v-icon icon="mdi-file-search-outline" size="48" class="mb-3" />
            <div>此處尚無檔案</div>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="deleteDialog" max-width="420" rounded="xl">
      <v-card rounded="xl" class="pa-2">
        <v-card-title class="text-h6">確認刪除</v-card-title>
        <v-card-text>
          確定刪除
          <span class="font-weight-medium">{{ pendingDelete }}</span>
          ？此操作無法復原。
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" rounded="xl" :disabled="deleting" @click="deleteDialog = false">取消</v-btn>
          <v-btn color="error" variant="flat" rounded="xl" :loading="deleting" @click="confirmDelete">刪除</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor" rounded="pill" timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </div>
</template>

<style scoped>
.folder-panel {
  position: sticky;
  top: 88px;
}

.folder-chips {
  overflow-x: auto;
  flex-wrap: nowrap;
}

:deep(.folder-item--active) {
  background-color: rgba(var(--v-theme-primary), 0.12);
  color: rgb(var(--v-theme-primary));
}

:deep(.folder-item--active .v-list-item__prepend .v-icon),
:deep(.folder-item--active .v-list-item-title) {
  color: rgb(var(--v-theme-primary));
  opacity: 1;
}
</style>
