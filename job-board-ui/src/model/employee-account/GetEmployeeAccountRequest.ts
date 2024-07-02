import PagedRequest from "../PagedRequest";

export default interface GetEmployeeAccountRequest extends PagedRequest {
    FirstName?: string,
    City?: string,
    Country?: string,
    LastName?: string
}