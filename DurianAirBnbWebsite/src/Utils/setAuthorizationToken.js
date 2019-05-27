import axios from "axios";
import {
  getJwtToken,
  getRefreshToken,
  saveJwtToken
} from "../api/AuthActions";
import createAuthRefreshInterceptor from "axios-auth-refresh";

// Function that will be called to refresh authorization
const refreshAuthLogic = () =>
  axios
    .post("http://localhost:55615/api/tokenrefresh", {
      accessToken: getJwtToken(),
      refreshToken: getRefreshToken()
    })
    .then(res => {
      const token = res.data.token;
      saveJwtToken(token);
      return Promise.resolve();
    });

createAuthRefreshInterceptor(axios, refreshAuthLogic);

axios.interceptors.request.use(request => {
  request.headers["Authorization"] = `Bearer ${getJwtToken()}`;
  return request;
});
