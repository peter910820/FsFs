<template>
  <div class="row main-block">
    <div class="col s12">
      <h1>資源伺服器目錄</h1>
    </div>
    <div class="col s12 file-field input-field">
      <div class="btn">
        <span><i class="material-icons">upload_file</i></span>
        <input type="file" name="file" @change="handleFileUpload" />
      </div>
      <div class="file-path-wrapper">
        <input class="file-path validate" type="text" placeholder="只允許上傳單一檔案" />
      </div>
    </div>
    <div class="col s12 input-field mobile-hidden">
      <select v-model="updateDirectory">
        <option class="validate" value="" disabled selected>選擇資料夾</option>
        <option v-for="(item, index) in directory" :key="index" :value="item">
          {{ (item as string).split("/")[1] }}
        </option>
      </select>
    </div>
    <div class="col s12 input-field mobile-display">
      <select v-model="updateDirectory" class="browser-default">
        <option class="validate" value="" disabled selected>選擇資料夾</option>
        <option v-for="(item, index) in directory" :key="index" :value="item">
          {{ (item as string).split("/")[1] }}
        </option>
      </select>
    </div>
    <div class="col s12">
      <button class="btn waves-effect waves-light" type="button" name="action" @click="upload">
        上傳
        <i class="material-icons right">send</i>
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { onMounted, ref, defineComponent } from "vue";
import { useRouter } from "vue-router";
import { initMaterialFormSelect } from "@/composables/useMaterial";
import axios from "axios";
import type { AxiosResponse } from "axios";

export default defineComponent({
  setup() {
    const router = useRouter();
    const directory = ref(null);
    let updateDirectory = ref<string | null>(null);
    const file = ref<File | null>(null);

    onMounted(async () => {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const getDirectory = async (): Promise<AxiosResponse | undefined> => {
        const apiUrl = "/api/directory";
        try {
          const response = await axios.get(apiUrl);
          return response;
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
        } catch (error: any) {
          return error.response;
        }
      };
      const response = await getDirectory();
      if (response && response.status === 200) {
        directory.value = response.data.data.slice(1);
      } else if (response) {
        router.push("/error");
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

      const formData = new FormData();
      formData.append("file", file.value);
      formData.append("directory", updateDirectory.value as string);

      try {
        await axios.post("/api/upload", formData, {
          headers: { "Content-Type": "multipart/form-data" },
        });
        alert("檔案上傳成功！");
      } catch (error) {
        console.log(error);
        if (axios.isAxiosError(error)) {
          router.push("/error");
        } else {
          router.push("/error");
        }
      }
    };

    return { directory, upload, file, handleFileUpload, updateDirectory };
  },
});
</script>

<style scoped></style>
