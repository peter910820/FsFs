import { defineStore } from "pinia";
import { ref } from "vue";

export const useDirectoryStore = defineStore("directory", () => {
  const directory = ref<Array<string>>();
  const set = (data: Array<string>) => {
    directory.value = data.slice(1);
  };

  return {
    directory,
    set,
  };
});
