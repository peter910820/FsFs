import { defineStore } from "pinia";
import { ref } from "vue";

export const useLoginStore = defineStore("login", () => {
  const status = ref<boolean>(false);
  const set = (newStatus: boolean) => {
    status.value = newStatus;
  };

  return {
    status,
    set,
  };
});
