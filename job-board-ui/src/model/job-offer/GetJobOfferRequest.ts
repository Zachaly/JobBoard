import PagedRequest from "../PagedRequest";

export default interface GetJobOfferRequest extends PagedRequest {
    CompanyId?: number,
    Location?: string,
    SearchCompanyName?: string,
    MinimalExpirationDate?: string,
    BusinessIds?: number[]
}