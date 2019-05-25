import Validator from "validator";
import isEmpty from "lodash/isEmpty";

export default function validateInputSignup(data) {
  let errors = {};

  if (Validator.isEmpty(data.username)) {
    errors.username = "This field is required";
  }
  if (!Validator.isEmail(data.username)) {
    errors.username = "Must be a valid email";
  }
  if (Validator.isEmpty(data.password)) {
    errors.password = "This field is required";
  }
  if (Validator.isEmpty(data.passwordConfirmation)) {
    errors.passwordConfirmation = "This field is required";
  }
  if (!Validator.isLength(data.password, { min: 8 })) {
    errors.passwordLength = "Password must be minimum 6 characters";
  }
  if (!Validator.equals(data.password, data.passwordConfirmation)) {
    errors.passwordConfirmation = "Passwords must match";
  }

  return {
    errors,
    isValid: isEmpty(errors)
  };
}
