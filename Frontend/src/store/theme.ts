import { readonly, ref } from "vue";
import type { ThemeInstance } from "vuetify";

export type ThemeName = "light" | "dark";

const STORAGE_KEY = "theme";

const readStored = (): ThemeName => {
  const saved = localStorage.getItem(STORAGE_KEY);
  return saved === "light" ? "light" : "dark";
};

const current = ref<ThemeName>(readStored());
let themeApi: ThemeInstance | null = null;

const persist = (name: ThemeName) => {
  current.value = name;
  localStorage.setItem(STORAGE_KEY, name);
};

const bind = (theme: ThemeInstance) => {
  themeApi = theme;
  theme.change(current.value);
};

const set = (name: ThemeName) => {
  persist(name);
  themeApi?.change(name);
};

const toggle = () => {
  set(current.value === "light" ? "dark" : "light");
};

export const themeStore = {
  current: readonly(current),
  bind,
  set,
  toggle,
  /** 供 createVuetify defaultTheme 使用 */
  initial: readStored,
};
