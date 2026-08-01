import "@mdi/font/css/materialdesignicons.css";
import "vuetify/styles";
import { createVuetify, type ThemeDefinition } from "vuetify";

import { themeStore } from "@/store/theme";

const lightTheme: ThemeDefinition = {
  dark: false,
  colors: {
    background: "#F1F5F9",
    surface: "#FFFFFF",
    "surface-bright": "#FFFFFF",
    "surface-light": "#E2E8F0",
    "surface-variant": "#CBD5E1",
    "on-surface-variant": "#334155",
    primary: "#0F766E",
    "primary-darken-1": "#115E59",
    secondary: "#475569",
    "secondary-darken-1": "#334155",
    accent: "#0891B2",
    error: "#DC2626",
    info: "#0284C7",
    success: "#059669",
    warning: "#D97706",
    "on-background": "#0F172A",
    "on-surface": "#0F172A",
    "on-primary": "#FFFFFF",
    "on-secondary": "#FFFFFF",
  },
};

const darkTheme: ThemeDefinition = {
  dark: true,
  colors: {
    background: "#0B1220",
    surface: "#151D2B",
    "surface-bright": "#1E293B",
    "surface-light": "#1E293B",
    "surface-variant": "#334155",
    "on-surface-variant": "#CBD5E1",
    primary: "#2DD4BF",
    "primary-darken-1": "#14B8A6",
    secondary: "#94A3B8",
    "secondary-darken-1": "#64748B",
    accent: "#22D3EE",
    error: "#F87171",
    info: "#38BDF8",
    success: "#34D399",
    warning: "#FBBF24",
    "on-background": "#E2E8F0",
    "on-surface": "#E2E8F0",
    "on-primary": "#042F2E",
    "on-secondary": "#0F172A",
  },
};

export default createVuetify({
  theme: {
    defaultTheme: themeStore.initial(),
    themes: {
      light: lightTheme,
      dark: darkTheme,
    },
  },
  defaults: {
    VBtn: {
      rounded: "lg",
    },
    VCard: {
      rounded: "xl",
    },
    VTextField: {
      rounded: "lg",
      variant: "outlined",
    },
    VListItem: {
      rounded: "lg",
    },
    VNavigationDrawer: {
      rounded: 0,
    },
    VDialog: {
      scrim: true,
    },
  },
});
