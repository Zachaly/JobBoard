export default interface ResponseModel {
  isSuccess: boolean;
  error?: string;
  validationErrors?: { [id: string]: string[] };
}
