export default interface EmployeeAccountModel {
    id: number,
    email: string,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    city?: string,
    country?: string,
    aboutMe?: string
}