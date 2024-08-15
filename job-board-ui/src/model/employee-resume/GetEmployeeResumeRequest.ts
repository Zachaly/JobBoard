import PagedRequest from "../PagedRequest";

export default interface GetEmployeeResumeRequest extends PagedRequest {
    EmployeeId?: number
}