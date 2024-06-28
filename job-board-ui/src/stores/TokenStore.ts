import LoginResponse from "@/model/LoginResponse";
import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import { AuthType } from "./AuthStore";

const ACCESS_TOKEN_STORAGE = "access_token";
const REFRESH_TOKEN_STORAGE = "refresh_token";

const useTokenStore = defineStore("token", () => {
  const accessToken: Ref<string | null> = ref(null);
  const refreshToken: Ref<string | null> = ref(null);

  const registerTokens = (loginData: LoginResponse) => {
    sessionStorage.setItem(ACCESS_TOKEN_STORAGE, loginData.authToken);
    sessionStorage.setItem(REFRESH_TOKEN_STORAGE, loginData.refreshToken);
  };

  const saveTokens = (authType: AuthType) => {
    localStorage.setItem(ACCESS_TOKEN_STORAGE, accessToken.value ?? "");
    localStorage.setItem(REFRESH_TOKEN_STORAGE, refreshToken.value ?? "");
    localStorage.setItem("auth_type", AuthType[authType]);
  };

  const clearTokens = () => {
    localStorage.setItem(ACCESS_TOKEN_STORAGE, "");
    localStorage.setItem(REFRESH_TOKEN_STORAGE, "");

    accessToken.value = "";
    refreshToken.value = "";
  };

  const loadTokensFromStorage = () => {
    accessToken.value = localStorage.getItem(ACCESS_TOKEN_STORAGE);
    refreshToken.value = localStorage.getItem(REFRESH_TOKEN_STORAGE);

    const authType = localStorage.getItem("auth_type");

    if (!authType) {
      return AuthType.NotAuthorized;
    }

    return AuthType[authType as keyof typeof AuthType];
  };

  const getAccessToken = () => sessionStorage.getItem(ACCESS_TOKEN_STORAGE)
  const getRefreshToken = () => sessionStorage.getItem(REFRESH_TOKEN_STORAGE)

  return {
    registerTokens,
    saveTokens,
    clearTokens,
    getAccessToken,
    getRefreshToken,
    loadTokensFromStorage,
  };
});

export default useTokenStore;
