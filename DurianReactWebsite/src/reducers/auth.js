import { SET_CURRENT_USER } from "../actions/actionTypes";
import isEmpty from "lodash/isEmpty";
import initialState from "../reducers/initialState";

export default function Auth(state = initialState.auth, action) {
  switch (action.type) {
    case SET_CURRENT_USER:
      return {
        ...state,
        isAuthenticated: !isEmpty(action.user),
        user: action.user,
        userId: action.userId
      };
    default:
      return state;
  }
}
