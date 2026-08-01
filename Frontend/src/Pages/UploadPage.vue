<script setup lang="ts">
import axios from "axios";
import { onMounted, ref } from "vue";

import BlockError from "@/components/BlockError.vue";
import { getDirectory } from "@/utils/apiHandler";

const directories = ref<string[]>([]);
const selectedDirectory = ref<string | null>(null);
const file = ref<File[]>([]);
const fileName = ref("");
const loading = ref(false);
const loadingDirs = ref(true);
const errorDirs = ref<string | null>(null);
const snackbar = ref(false);
const snackbarText = ref("");
const snackbarColor = ref("success");

const showMessage = (text: string, color = "success") => {
  snackbarText.value = text;
  snackbarColor.value = color;
  snackbar.value = true;
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

onMounted(loadDirectories);

const isValidLinuxFileName = (name: string) => {
  if (!name || name.trim() === "") return false;
  if (name.includes("/") || name.includes("\0")) return false;
  if (/[*?"<>|]/.test(name)) return false;
  if (name === "." || name === "..") return false;
  if (name.length > 255) return false;
  return true;
};

const upload = async () => {
  const selectedFile = file.value[0];
  if (!selectedFile) {
    showMessage("請選擇檔案", "warning");
    return;
  }
  if (!selectedDirectory.value) {
    showMessage("請選擇上傳資料夾", "warning");
    return;
  }
  if ([".js", ".exe", ".dll", ".sh"].some((ext) => selectedFile.name.toLowerCase().endsWith(ext))) {
    showMessage("不允許該檔案上傳", "error");
    return;
  }

  const formData = new FormData();
  if (fileName.value.trim() !== "") {
    if (!isValidLinuxFileName(fileName.value.trim())) {
      showMessage("檔名有非法字元，請修改檔名", "error");
      return;
    }
    const extension = selectedFile.name.includes(".")
      ? selectedFile.name.slice(selectedFile.name.lastIndexOf("."))
      : "";
    formData.append("file", new File([selectedFile], fileName.value.trim() + extension, { type: selectedFile.type }));
  } else {
    formData.append("file", selectedFile);
  }
  formData.append("directory", selectedDirectory.value);

  loading.value = true;
  try {
    const apiBase = import.meta.env.VITE_API_DOMAIN || "";
    await axios.post(`${apiBase}/api/upload/${selectedDirectory.value}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
      withCredentials: true,
    });
    showMessage("檔案上傳成功！");
    file.value = [];
    fileName.value = "";
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) {
      showMessage(error.response?.data?.msg || error.message, "error");
    } else if (error instanceof Error) {
      showMessage(`例外錯誤 ${error.message}`, "error");
    } else {
      showMessage("上傳失敗", "error");
    }
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <v-row justify="center">
    <v-col cols="12" md="8" lg="6">
      <v-card class="overflow-hidden" rounded="xl" elevation="2">
        <div class="upload-hero pa-8">
          <div class="d-flex align-center ga-4">
            <v-avatar color="primary" size="56" variant="tonal">
              <v-icon icon="mdi-cloud-upload" size="28" />
            </v-avatar>
            <div>
              <div class="text-h5 font-weight-bold">上傳檔案</div>
              <div class="text-body-2 text-medium-emphasis">選擇資料夾與檔案，完成資源上傳</div>
            </div>
          </div>
        </div>

        <v-card-text class="pa-8">
          <v-skeleton-loader v-if="loadingDirs" type="heading" class="mb-5" />
          <BlockError v-else-if="errorDirs" class="mb-5" :message="errorDirs" @retry="loadDirectories" />
          <v-select
            v-else
            v-model="selectedDirectory"
            class="mb-5"
            :items="directories"
            label="上傳資料夾"
            placeholder="選擇資料夾"
            prepend-inner-icon="mdi-folder-outline"
            hide-details="auto"
            clearable
          />

          <v-file-input
            v-model="file"
            class="mb-5"
            label="選擇檔案"
            placeholder="只允許上傳單一檔案"
            prepend-icon=""
            prepend-inner-icon="mdi-file-upload-outline"
            show-size
            hide-details="auto"
            :multiple="false"
          />

          <v-text-field
            v-model="fileName"
            class="mb-6"
            label="檔案名稱（選填，無須副檔名）"
            placeholder="留空則使用原始檔名"
            prepend-inner-icon="mdi-form-textbox"
            hide-details="auto"
          />

          <v-alert class="mb-6" type="info" variant="tonal" rounded="lg" density="comfortable">
            不允許上傳 `.js`、`.exe`、`.dll`、`.sh` 檔案。
          </v-alert>

          <v-btn
            block
            size="large"
            rounded="xl"
            color="primary"
            variant="flat"
            prepend-icon="mdi-send"
            :loading="loading"
            :disabled="!!errorDirs"
            @click="upload"
          >
            上傳
          </v-btn>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>

  <v-snackbar v-model="snackbar" :color="snackbarColor" rounded="pill" timeout="3000">
    {{ snackbarText }}
  </v-snackbar>
</template>

<style scoped>
.upload-hero {
  background: linear-gradient(145deg, rgba(var(--v-theme-primary), 0.16) 0%, rgba(var(--v-theme-surface), 1) 72%);
}
</style>
