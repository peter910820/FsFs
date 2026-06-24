import axios from "axios";
// type
import type { AxiosResponse } from "axios";

export const getDirectory = async (): Promise<AxiosResponse> => {
  try {
    let url = "/api/directories";
    if (import.meta.env.VITE_API_DOMAIN) {
      url = import.meta.env.VITE_API_DOMAIN + "/api/directories";
    }
    const response = await axios.get(url);
    return response;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (error: any) {
    return error.response;
  }
};

export const getFile = async (): Promise<AxiosResponse> => {
  let url = "/api/files";
  if (import.meta.env.VITE_API_DOMAIN) {
    url = import.meta.env.VITE_API_DOMAIN + "/api/files";
  }
  try {
    const response = await axios.get(url);
    return response;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (error: any) {
    return error.response;
  }
};
