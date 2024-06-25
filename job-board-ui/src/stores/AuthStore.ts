import AdminAccountModel from "@/model/admin-account/AdminAccountModel";
import CompanyAccountModel from "@/model/company-account/CompanyAccountModel";
import EmployeeAccountModel from "@/model/employee-account/EmployeeAccountModel";
import LoginResponse from "@/model/LoginResponse";
import axios from "axios";
import { defineStore } from "pinia";
import { ref, Ref } from "vue";

export enum AuthType {
    Admin,
    Company,
    Employee,
    NotAuthorized
}

const useAuthStore = defineStore('auth', () => {
    const currentAuthType: Ref<AuthType> = ref(AuthType.NotAuthorized)
    const currentUserId: Ref<number> = ref(0)
    const authToken = ref('')
    const adminData: Ref<AdminAccountModel | null> = ref(null)
    const employeeData: Ref<EmployeeAccountModel | null> = ref(null)
    const companyData: Ref<CompanyAccountModel | null> = ref(null)

    const authorize = (loginResponse: LoginResponse, type: AuthType) => {
        currentAuthType.value = type
        currentUserId.value = loginResponse.userId
        authToken.value = loginResponse.authToken
        axios.defaults.headers.common.Authorization = `Bearer ${authToken.value}`

        adminData.value = null
        employeeData.value = null
        companyData.value = null

        if(type == AuthType.Admin) {
            axios.get<AdminAccountModel>(`admin-account/${loginResponse.userId}`).then(res => adminData.value = res.data)
        } else if(type == AuthType.Company) {
            axios.get<CompanyAccountModel>(`company-account/${loginResponse.userId}`).then(res => companyData.value = res.data)
        } else if(type == AuthType.Employee) {
            axios.get<EmployeeAccountModel>(`employee-account/${loginResponse.userId}`).then(res => employeeData.value = res.data)
        }
    }

    const logout = () => {
        currentUserId.value = 0
        currentAuthType.value = AuthType.NotAuthorized
        authToken.value = ''
        axios.defaults.headers.common.Authorization = ''
        adminData.value = null
        employeeData.value = null
        companyData.value = null
    }

    return { authorize, currentAuthType, currentUserId, logout }
})


export default useAuthStore