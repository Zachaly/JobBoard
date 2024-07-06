export default interface AddJobOfferRequest {
    companyId: number,
    expirationTimestamp: number,
    title: string,
    description: string,
    location: string,
    requirements: string[]
}