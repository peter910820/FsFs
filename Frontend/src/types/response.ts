// 對應後端 ApiResponse<'T>（Data 為 option，無值時為 null）
export interface ResponseType<T = unknown> {
  statusCode: number;
  msg: string;
  data: T | null;
}
