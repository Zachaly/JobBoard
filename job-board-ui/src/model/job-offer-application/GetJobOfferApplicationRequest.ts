import PagedRequest from "../PagedRequest";

export default interface GetJobOfferApplicationRequest extends PagedRequest {
    OfferId?: number,
    EmployeeId?: number,
    State?: JobOfferApplicationState,
    MinimalApplicationDate?: string
}