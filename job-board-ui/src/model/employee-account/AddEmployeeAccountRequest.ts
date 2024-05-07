export default interface AddEmployeeAccountRequest {
    email: string,
    password: string,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    aboutMe?: string,
    country?: string,
    city?: string
}