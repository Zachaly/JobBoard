import JobOfferWorkType from "../enum/JobOfferWorkType";

export default interface UpdateJobOfferRequest {
    id: number,
    title: string,
    expirationTimestamp: number,
    description: string,
    location: string,
    businessId?: number,
    workType: JobOfferWorkType
}