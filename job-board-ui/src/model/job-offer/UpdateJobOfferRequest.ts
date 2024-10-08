import JobOfferWorkType from "../enum/JobOfferWorkType";
import SalaryType from "../enum/SalaryType";
import WorkExperienceLevel from "../enum/WorkExperienceLevel";

export default interface UpdateJobOfferRequest {
    id: number,
    title: string,
    expirationTimestamp: number,
    description: string,
    location: string,
    businessId?: number,
    workType: JobOfferWorkType
    minSalary?: number,
    maxSalary?: number,
    salaryType?: SalaryType,
    experienceLevel?: WorkExperienceLevel
}