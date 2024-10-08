import JobOfferWorkType from "../enum/JobOfferWorkType";
import SalaryType from "../enum/SalaryType";
import WorkExperienceLevel from "../enum/WorkExperienceLevel";

export default interface AddJobOfferRequest {
    companyId: number,
    expirationTimestamp: number,
    title: string,
    description: string,
    location: string,
    requirements: string[],
    businessId?: number,
    tags: string[],
    workType: JobOfferWorkType,
    minSalary?: number,
    maxSalary?: number,
    salaryType?: SalaryType,
    experienceLevel?: WorkExperienceLevel
}