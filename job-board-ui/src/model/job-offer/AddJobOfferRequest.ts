import JobOfferWorkType from "../enum/JobOfferWorkType";

export default interface AddJobOfferRequest {
    companyId: number,
    expirationTimestamp: number,
    title: string,
    description: string,
    location: string,
    requirements: string[],
    businessId?: number,
    tags: string[],
    workType: JobOfferWorkType
}