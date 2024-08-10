import JobOfferApplicationState from "../enum/JobOfferApplicationState";

export default interface UpdateJobOfferApplicationRequest {
    id: number,
    state: JobOfferApplicationState
}