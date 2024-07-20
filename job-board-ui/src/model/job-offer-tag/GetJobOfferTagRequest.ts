import PagedRequest from "../PagedRequest";

export default interface GetJobOfferTagRequest extends PagedRequest {
    OfferId?: number,
    Tag?: string
}