// 泛型，定義共通Response
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export interface ResponseType<T = any> {
  statusCode: number;
  msg: string;
  data: T;
}
