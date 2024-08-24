import JobOfferWorkType from "../enum/JobOfferWorkType";
import PagedRequest from "../PagedRequest";
import WorkExperienceLevel from '../enum/WorkExperienceLevel';

export default interface GetJobOfferRequest extends PagedRequest {
    CompanyId?: number,
    Location?: string,
    SearchCompanyName?: string,
    MinimalExpirationDate?: string,
    BusinessIds?: number[],
    Tags?: string[],
    WorkType?: JobOfferWorkType,
    ExperienceLevel?: WorkExperienceLevel[]
}