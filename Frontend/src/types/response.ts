import type { UserType } from "@/types/user";

// 泛型，定義共通Response
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export interface ResponseType<T = any> {
  message: string;
  data: T;
}

export type LoginResponseType = ResponseType<UserType>;
