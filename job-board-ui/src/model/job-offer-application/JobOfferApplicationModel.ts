import EmployeeAccountModel from "../employee-account/EmployeeAccountModel";
import JobOfferApplicationState from "../enum/JobOfferApplicationState";

export default interface JobOfferApplicationModel {
    id: number,
    state: JobOfferApplicationState,
    employee: EmployeeAccountModel,
    offerId: number,
    applicationDate: string,
    offerTitle: string
}