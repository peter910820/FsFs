<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { initMaterialFormSelect } from "@/composables/useMaterial";
import axios from "axios";

import { getDirectory } from "@/utils/apiHandler";

const router = useRouter();
const directory = ref(null);
const updateDirectory = ref<string | null>(null);
const file = ref<File | null>(null);
const fileName = ref<string>("");

onMounted(async () => {
  const response = await getDirectory();
  if (response && response.status === 200) {
    directory.value = response.data.data;
  } else {
    router.push("/error");
  }
  initMaterialFormSelect();
});

const handleFileUpload = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    file.value = target.files[0];
  }
};

const upload = async () => {
  if (!file.value) {
    alert("請選擇檔案");
    return;
  }
  if (!updateDirectory.value) {
    alert("請選擇上傳資料夾");
    return;
  }

  if ([".js", ".exe", ".dll", ".sh"].some((ext) => (file.value?.name ?? "").toLowerCase().endsWith(ext))) {
    alert("不允許該檔案上傳");
    return;
  }

  // 包裝成FormData
  const formData = new FormData();
  if (fileName.value.trim() !== "") {
    if (!isValidLinuxFileName(fileName.value.trim())) {
      alert("檔名有非法字元，請修改檔名");
      return;
    }
    // 取得原檔副檔名
    const originalName = file.value.name;
    const extension = originalName.includes(".") ? originalName.slice(originalName.lastIndexOf(".")) : "";
    formData.append("file", new File([file.value], fileName.value.trim() + extension, { type: file.value.type }));
  } else {
    formData.append("file", file.value);
  }
  formData.append("directory", updateDirectory.value as string);

  try {
    await axios.post(import.meta.env.VITE_API_DOMAIN + "/api/upload/" + updateDirectory.value, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    alert("檔案上傳成功！");
  } catch (error: any) {
    if (axios.isAxiosError(error)) {
      sessionStorage.setItem("errorMsg", error.response?.data?.msg || error.message);
    } else {
      sessionStorage.setItem("errorMsg", `例外錯誤 ${error.message}`);
    }
    router.push("/error");
  }
};

const isValidLinuxFileName = (name: string) => {
  // 空字串、null、undefined 都視為非法
  if (!name || name.trim() === "") return false;

  // 禁止 / 與 null 字元
  if (name.includes("/") || name.includes("\0")) return false;

  // 避免特殊字元（可選）
  const illegalPattern = /[*?"<>|]/;
  if (illegalPattern.test(name)) return false;

  // 避免 . 與 ..
  if (name === "." || name === "..") return false;

  // 長度檢查
  if (name.length > 255) return false;

  return true;
};
</script>

<template>
  <div class="row main-block">
    <div class="col s12">
      <h1>資源伺服器目錄</h1>
    </div>
    <div class="col s12 file-field input-field">
      <div class="col s1 button-common flex-btn">
        <span><i class="material-icons">upload_file</i></span>
        <input type="file" name="file" @change="handleFileUpload" />
      </div>
      <div class="file-path-wrapper">
        <input class="file-path validate" type="text" placeholder="只允許上傳單一檔案" />
      </div>
    </div>
    <div class="col s12 input-field">
      <input id="file_name" type="text" class="validate" v-model="fileName" />
      <label for="file_name">檔案名稱(無須副檔名)</label>
    </div>
    <div class="col s12 input-field mobile-hidden">
      <select v-model="updateDirectory">
        <option class="validate" value="" disabled selected>選擇資料夾</option>
        <option v-for="(item, index) in directory" :key="index" :value="item">
          {{ item }}
        </option>
      </select>
    </div>
    <div class="col s12 input-field mobile-display">
      <select v-model="updateDirectory" class="browser-default">
        <option class="validate" value="" disabled selected>選擇資料夾</option>
        <option v-for="(item, index) in directory" :key="index" :value="item">
          {{ item }}
        </option>
      </select>
    </div>
    <div class="col s12">
      <button class="button-common flex-btn" type="button" name="action" @click="upload">
        上傳
        <i class="material-icons right">send</i>
      </button>
    </div>
  </div>
</template>

<style scoped></style>
