export default interface AddCompanyAccountRequest {
  email: string;
  password: string;
  name: string;
  city: string;
  postalCode: string;
  address: string;
  country: string;
  contactEmail: string;
  about?: string;
}
