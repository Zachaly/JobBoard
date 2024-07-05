import CompanyAccountModel from "../company-account/CompanyAccountModel";

export default interface JobOfferModel {
    id: number,
    title: string,
    description: string,
    location: string,
    expirationDate: string,
    creationDate: string,
    company: CompanyAccountModel
}