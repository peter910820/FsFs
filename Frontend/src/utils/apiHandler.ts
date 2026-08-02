import axios from "axios";
import type { AxiosResponse } from "axios";

import type { ResponseType } from "@/types/response";
import type { RecentFileItem } from "@/types/file";
import type { LoginResponse } from "@/types/user";

export const apiUrl = (path: string) => {
  const normalized = path.startsWith("/") ? path : `/${path}`;
  const base = import.meta.env.VITE_API_DOMAIN || "";
  return `${base}/api${normalized}`;
};

export const getErrorMessage = (error: unknown, fallback: string) => {
  if (axios.isAxiosError(error)) {
    return error.response?.data?.msg || error.message || fallback;
  }
  if (error instanceof Error) {
    return error.message || fallback;
  }
  return fallback;
};

export const getDirectory = async (): Promise<AxiosResponse> => {
  try {
    return await axios.get(apiUrl("/directories"));
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) return error.response as AxiosResponse;
    throw error;
  }
};

export const getFile = async (dir?: string): Promise<AxiosResponse> => {
  const url = dir ? apiUrl(`/files?dir=${encodeURIComponent(dir)}`) : apiUrl("/files");
  try {
    return await axios.get(url);
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) return error.response as AxiosResponse;
    throw error;
  }
};

export const getRecentFiles = async (limit = 10): Promise<AxiosResponse<ResponseType<RecentFileItem[]>>> => {
  try {
    return await axios.get(apiUrl(`/files/recent?limit=${limit}`));
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) return error.response as AxiosResponse<ResponseType<RecentFileItem[]>>;
    throw error;
  }
};

export const deleteFile = async (fileName: string) => {
  return axios.delete<ResponseType<string[]>>(apiUrl("/file"), {
    data: { fileName },
    headers: { "Content-Type": "application/json" },
    withCredentials: true,
  });
};

export const uploadFile = async (directory: string, formData: FormData) => {
  return axios.post(apiUrl(`/upload/${directory}`), formData, {
    headers: { "Content-Type": "multipart/form-data" },
    withCredentials: true,
  });
};

export const login = async (username: string, password: string) => {
  return axios.post<ResponseType<LoginResponse>>(apiUrl("/login"), { username, password }, { withCredentials: true });
};

export const authCheck = async () => {
  return axios.post<ResponseType<null>>(apiUrl("/auth"), {}, { withCredentials: true });
};
