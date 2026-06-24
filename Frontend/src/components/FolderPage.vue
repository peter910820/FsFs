<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import type { ResponseType } from "@/types/response";

import { getDirectory, getFile } from "@/utils/apiHandler";

const router = useRouter();

const dirData = ref<string[]>([]);
const fileData = ref<string[]>([]);

const expandDetails = async (folder: string) => {
  const apiUrl = import.meta.env.VITE_API_DOMAIN
    ? `${import.meta.env.VITE_API_DOMAIN}/api/files?dir=${folder}`
    : `/api/files?dir=${folder}`;
  try {
    const response = await axios.get<ResponseType<string[]>>(apiUrl);
    fileData.value = response.data.data;
  } catch (error: any) {
    if (axios.isAxiosError(error)) {
      sessionStorage.setItem("errorMsg", error.response?.data?.msg || error.message);
    } else {
      sessionStorage.setItem("errorMsg", `例外錯誤 ${error.message}`);
    }
    router.push("/error");
  }
};

const goToUrl = async (url: string) => {
  window.location.href = import.meta.env.VITE_STATIC_FILE_DOMAIN + "/" + url;
};

const deleteFile = async (url: string) => {
  if (confirm("確定刪除?")) {
    const apiUrl = import.meta.env.VITE_API_DOMAIN ? `${import.meta.env.VITE_API_DOMAIN}/api/file` : `/api/file`;
    try {
      const response = await axios.delete<ResponseType<string[]>>(apiUrl, {
        data: { fileName: url },
        headers: { "Content-Type": "application/json" },
      });
      fileData.value = response.data.data;
    } catch (error: any) {
      if (axios.isAxiosError(error)) {
        sessionStorage.setItem("errorMsg", error.response?.data?.msg || error.message);
      } else {
        sessionStorage.setItem("errorMsg", `例外錯誤 ${error.message}`);
      }
      router.push("/error");
    }
  }
};

onMounted(async () => {
  let response = await getDirectory();
  if (response && response.status === 200) {
    dirData.value = response.data.data;
  } else {
    sessionStorage.setItem("errorMsg", response?.data?.msg);
    router.push("/error");
  }
  response = await getFile();
  if (response && response.status === 200) {
    fileData.value = response.data.data;
  } else {
    sessionStorage.setItem("errorMsg", response?.data?.msg);
    router.push("/error");
  }
});
</script>

<template>
  <div class="row main-block">
    <h1 class="center">伺服器資源</h1>
    <div class="col l4 m12 s12 file-block input-field">
      <div class="row">
        <div class="col s12 folder-block-title">📁選擇資料夾</div>
        <div class="col s12 folder" v-for="(item, index) in dirData" :key="index" :value="item">
          <input type="button" class="button-folder" @click="expandDetails(item)" :value="item" />
        </div>
      </div>
    </div>
    <div class="col l8 m12 s12 file-block input-field">
      <div class="row">
        <div class="col s12 file-block-title">📂資料夾內容</div>
        <div
          class="col s12 wow animate__fadeInRightBig floatup-div file ellipsis"
          v-for="(item, index) in fileData"
          @click="goToUrl(item)"
          :key="index"
          :value="item"
        >
          <a>
            <div v-if="['.png', '.jpg', '.jpeg'].some((ext) => item.toLowerCase().endsWith(ext))">🖼️{{ item }}</div>
            <div v-else-if="['.zip', '.rar', '.7z', '.tar.gz'].some((ext) => item.toLowerCase().endsWith(ext))">
              🗃️{{ item }}
            </div>
            <div v-else-if="['.go', '.py', '.fs', '.cs'].some((ext) => item.toLowerCase().endsWith(ext))">
              💻{{ item }}
            </div>
            <div v-else-if="['.xp3'].some((ext) => item.toLowerCase().endsWith(ext))">⚙️{{ item }}</div>
            <div v-else>📃{{ item }}</div>
          </a>
          <input type="button" class="button-delete" @click.stop="deleteFile(item)" value="刪除" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
a {
  font-size: 25px;
}
.center {
  text-align: center;
}
.folder {
  margin-top: 10px;
  > input {
    width: 150px;
    height: 50px;
    font-size: 20px;
  }
}
.file {
  border: 2px solid skyblue;
  border-radius: 100px;
  margin-bottom: 10px;
  padding: 20px;
  cursor: pointer;
}
.folder-block-title {
  font-size: 30px;
}
.file-block-title {
  font-size: 30px;
}
.ellipsis {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.button-delete {
  background-color: #ea4c89;
  border-radius: 8px;
  border-style: none;
  box-sizing: border-box;
  color: #ffffff;
  cursor: pointer;
  display: inline-block;
  font-family: "Haas Grot Text R Web", "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-size: 14px;
  font-weight: 500;
  height: 40px;
  line-height: 20px;
  list-style: none;
  margin: 0;
  outline: none;
  padding: 10px 16px;
  position: relative;
  text-align: center;
  text-decoration: none;
  transition: color 100ms;
  vertical-align: baseline;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

.button-delete:hover,
.button-delete:focus {
  background-color: #f082ac;
}
</style>
