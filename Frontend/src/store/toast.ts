import { readonly, ref } from "vue";

const visible = ref(false);
const text = ref("");
const color = ref("primary");

const show = (message: string, toastColor = "warning") => {
  text.value = message;
  color.value = toastColor;
  visible.value = true;
};

const hide = () => {
  visible.value = false;
};

export const toastStore = {
  visible,
  text: readonly(text),
  color: readonly(color),
  show,
  hide,
};
