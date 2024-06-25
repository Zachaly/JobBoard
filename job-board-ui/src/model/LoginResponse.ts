import ResponseModel from "./ResponseModel";

export default interface LoginResponse extends ResponseModel {
    userId: number,
    authToken: string
}