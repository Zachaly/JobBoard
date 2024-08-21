import CompanyAccountModel from "../company-account/CompanyAccountModel";
import JobOfferWorkType from "../enum/JobOfferWorkType";
import JobOfferRequirementModel from "../job-offer-requirement/JobOfferRequirementModel";
import JobOfferTagModel from "../job-offer-tag/JobOfferTagModel";

export default interface JobOfferModel {
    id: number,
    title: string,
    description: string,
    location: string,
    expirationDate: string,
    creationDate: string,
    company: CompanyAccountModel,
    requirements: JobOfferRequirementModel[],
    businessName?: string,
    businessId?: number,
    tags: JobOfferTagModel[],
    workType: JobOfferWorkType
}