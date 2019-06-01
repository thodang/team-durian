import axios from "axios";
import jwtDecode from "jwt-decode";
import { SET_CURRENT_USER } from "../actions/actionTypes";

const baseUrl = "https://cmpe-term-project.azurewebsites.net";
//const baseUrl = "http://localhost:55615";

export function setCurrentUser(user, userId) {
  return {
    type: SET_CURRENT_USER,
    user,
    userId
  };
}

export function logout() {
  return async dispatch => {
    await axios.post(baseUrl + "/api/logout", {
      accessToken: getJwtToken(),
      refreshToken: getRefreshToken()
    });
    localStorage.removeItem("durianToken");
    localStorage.removeItem("durianRef");
    dispatch(setCurrentUser({}, {}));
    window.location.reload(true);
  };
}

export function requestPasswordReset(data) {
  return async dispatch => {
    await axios.post(baseUrl + "/api/login/forgotpassword", data);
  };
}

export function resetPassword(data) {
  console.log("Calling resetPassword");
  console.log("token: " + data.token);
  return async dispatch => {
    await axios.post(`${baseUrl}/api/login/passwordreset/${data.token}`, data);
  };
}

export function login(data) {
  return async dispatch => {
    const response = await axios.post(baseUrl + "/api/login", data);
    const accessToken = response.data.accessToken;
    const refreshToken = response.data.refreshToken;
    saveJwtToken(accessToken);
    saveRefreshToken(refreshToken);
    dispatch(
      setCurrentUser(
        getUserNameFromToken(accessToken),
        getUserIdFromToken(accessToken)
      )
    );
  };
}

export function signup(data) {
  return async dispatch => {
    const response = await axios.post(baseUrl + "/api/signup", data);
    const accessToken = response.data.accessToken;
    const refreshToken = response.data.refreshToken;
    saveJwtToken(accessToken);
    saveRefreshToken(refreshToken);
    dispatch(
      setCurrentUser(
        getUserNameFromToken(accessToken),
        getUserIdFromToken(accessToken)
      )
    );
  };
}

export function isUserExists(data) {
  return () => {
    return axios.post(baseUrl + "/api/signup/checkuser", {
      username: data
    });
  };
}

export function getUserNameFromToken(token) {
  var payload = jwtDecode(token);
  return payload["user"];
}

export function getUserIdFromToken(token) {
  var payload = jwtDecode(token);
  return payload["uid"];
}

export function getJwtToken() {
  return localStorage.getItem("durianToken");
}

export function getRefreshToken() {
  return localStorage.getItem("durianRef");
}

export function saveJwtToken(token) {
  localStorage.setItem("durianToken", token);
}

export function saveRefreshToken(refreshToken) {
  localStorage.setItem("durianRef", refreshToken);
}
