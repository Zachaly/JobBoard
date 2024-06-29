import AdminAccountModel from "@/model/admin-account/AdminAccountModel";
import CompanyAccountModel from "@/model/company-account/CompanyAccountModel";
import EmployeeAccountModel from "@/model/employee-account/EmployeeAccountModel";
import LoginResponse from "@/model/LoginResponse";
import axios, { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref, Ref } from "vue";
import useTokenStore from "./TokenStore";
import RefreshTokenRequest from "@/model/RefreshTokenRequest";
import RevokeRefreshTokenRequest from "@/model/RevokeRefreshTokenRequest";

export enum AuthType {
  Admin = 1,
  Company = 2,
  Employee = 3,
  NotAuthorized = 4,
}

const authTypeToEndpointPart = (type: AuthType) => {
  if (type == AuthType.Admin) {
    return "admin";
  }

  if (type == AuthType.Company) {
    return "company";
  }

  if (type == AuthType.Employee) {
    return "employee";
  }

  return "";
};

const useAuthStore = defineStore("auth", () => {
  const currentAuthType: Ref<AuthType> = ref(AuthType.NotAuthorized);
  const currentUserId: Ref<number> = ref(0);
  const adminData: Ref<AdminAccountModel | null> = ref(null);
  const employeeData: Ref<EmployeeAccountModel | null> = ref(null);
  const companyData: Ref<CompanyAccountModel | null> = ref(null);
  const rememberMe = ref(false);

  const tokenStore = useTokenStore();

  const authorize = (
    loginResponse: LoginResponse,
    type: AuthType,
    remember = false
  ) => {
    currentAuthType.value = type;
    currentUserId.value = loginResponse.userId;
    tokenStore.registerTokens(loginResponse);
    rememberMe.value = remember;
    axios.defaults.headers.common.Authorization = `Bearer ${tokenStore.getAccessToken()}`;

    console.log(currentAuthType.value);

    axios.interceptors.response.use(
      (response) => response,
      async (error: AxiosError) => {
        console.log(error);
        if (error.response?.status == 401) {
          const success = await refreshTokens(currentAuthType.value);
          if (!success) {
            return Promise.reject(error);
          }

          const config = error.config!;
          config.headers.Authorization = `Bearer ${tokenStore.getAccessToken()}`;

          return axios(config);
        }

        return Promise.reject(error);
      }
    );

    adminData.value = null;
    employeeData.value = null;
    companyData.value = null;

    if (type == AuthType.Admin) {
      axios
        .get<AdminAccountModel>(`admin-account/${loginResponse.userId}`)
        .then((res) => (adminData.value = res.data));
    } else if (type == AuthType.Company) {
      axios
        .get<CompanyAccountModel>(`company-account/${loginResponse.userId}`)
        .then((res) => (companyData.value = res.data));
    } else if (type == AuthType.Employee) {
      axios
        .get<EmployeeAccountModel>(`employee-account/${loginResponse.userId}`)
        .then((res) => (employeeData.value = res.data));
    }

    if (remember) {
      tokenStore.saveTokens(type);
    }
  };

  const refreshTokens = (type: AuthType) =>
    new Promise<LoginResponse | null>((resolve) => {
      if (type == AuthType.NotAuthorized) {
        resolve(null);
      }

      const typeString = authTypeToEndpointPart(type);

      const request: RefreshTokenRequest = {
        accessToken: tokenStore.getAccessToken() ?? "",
        refreshToken: tokenStore.getRefreshToken() ?? "",
      };

      axios
        .post<LoginResponse>(`refresh-token/${typeString}`, request)
        .then((res) => {
          tokenStore.registerTokens(res.data);
          resolve(res.data);
        })
        .catch(() => resolve(null));
    });

  const logout = () => {
    const revokeTokenRequest: RevokeRefreshTokenRequest = {
      token: tokenStore.getRefreshToken() ?? "",
    };

    axios.patch(
      `refresh-token/${authTypeToEndpointPart(currentAuthType.value)}/revoke`,
      revokeTokenRequest
    );

    currentUserId.value = 0;
    currentAuthType.value = AuthType.NotAuthorized;
    axios.defaults.headers.common.Authorization = "";
    adminData.value = null;
    employeeData.value = null;
    companyData.value = null;

    tokenStore.clearTokens();
  };

  const loadSavedUser = async () => {
    const type = tokenStore.loadTokensFromStorage();

    if (
      tokenStore.getAccessToken() == null ||
      tokenStore.getRefreshToken() == null ||
      type == AuthType.NotAuthorized
    ) {
      return;
    }

    const data = await refreshTokens(type);

    if (!data) {
      return;
    }

    authorize(data, type, true);
  };

  return { authorize, currentAuthType, currentUserId, logout, loadSavedUser };
});

export default useAuthStore;
