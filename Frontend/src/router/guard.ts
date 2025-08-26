import axios from "axios";
// type
import type { AxiosResponse } from "axios";
import type { RouteLocationNormalized, NavigationGuardNext } from "vue-router";

import { useDirectoryStore } from "../store/file";

// support function
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

const getDataEntryPoint = async (
  to: RouteLocationNormalized,
  _from: RouteLocationNormalized,
  next: NavigationGuardNext,
) => {
  let response;
  switch (to.name) {
    case "folder":
      response = await getDirectory();
      break;
  }
  if (response && response.status === 200) {
    setStore(response, to);
    next();
  } else if (response) {
    next("/error");
  } else {
    next("/error");
  }
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const setStore = async (response: AxiosResponse<any, any>, to: RouteLocationNormalized) => {
  let store;
  switch (to.name) {
    case "folder":
      store = useDirectoryStore();
      store.set(response.data.data);
      break;
  }
};

export { getDataEntryPoint };
