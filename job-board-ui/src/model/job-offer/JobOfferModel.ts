import CompanyAccountModel from "../company-account/CompanyAccountModel";
import JobOfferRequirementModel from "../job-offer-requirement/JobOfferRequirementModel";

export default interface JobOfferModel {
    id: number,
    title: string,
    description: string,
    location: string,
    expirationDate: string,
    creationDate: string,
    company: CompanyAccountModel,
    requirements: JobOfferRequirementModel[]
}