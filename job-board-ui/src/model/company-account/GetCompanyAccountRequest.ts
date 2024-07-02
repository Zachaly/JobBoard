import PagedRequest from "../PagedRequest";

export default interface GetCompanyAccountRequest extends PagedRequest {
    Country?: string,
    Name?: string,
    City?: string,
    PostalCode?: string
}