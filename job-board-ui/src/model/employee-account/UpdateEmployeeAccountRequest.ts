export default interface UpdateEmployeeAccountRequest {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  city?: string;
  country?: string;
  aboutMe?: string;
}
