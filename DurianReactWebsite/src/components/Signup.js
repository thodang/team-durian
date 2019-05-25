import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import TextFieldGroup from "../components/TextFieldGroup";
import validateInput from "../Validation/validateInputSignup";
import { signup, isUserExists } from "../api/AuthActions";
import { addFlashMessage } from "../actions/flashMessages";

class SignupForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: "",
      firstname: "",
      lastname: "",
      password: "",
      passwordConfirmation: "",
      errors: {},
      isLoading: false,
      invalid: false
    };

    this.onChange = this.onChange.bind(this);
    this.onSubmit = this.onSubmit.bind(this);
    this.checkUserExists = this.checkUserExists.bind(this);
  }

  onChange(e) {
    this.setState({ [e.target.name]: e.target.value });
  }

  isValid() {
    const { errors, isValid } = validateInput(this.state);

    if (!isValid) {
      this.setState({ errors });
    }

    return isValid;
  }

  checkUserExists(e) {
    const field = e.target.name;
    const val = e.target.value;
    if (val !== "") {
      this.props.isUserExists(val).then(res => {
        let errors = this.state.errors;
        let invalid;
        if (res.data) {
          errors[field] = "Email address already registered";
          invalid = true;
        } else {
          errors[field] = "";
          invalid = false;
        }
        this.setState({ errors, invalid });
      });
    }
  }

  onSubmit(e) {
    e.preventDefault();

    if (this.isValid()) {
      this.setState({ errors: {}, isLoading: true });
      this.props.signup(this.state).then(
        () => {
          addFlashMessage({
            type: "success",
            text: "You signed up successfully. Welcome!"
          });
          this.props.history.push("/home");
        },
        err => this.setState({ errors: err.response.data, isLoading: false })
      );
    }
  }

  render() {
    const { errors } = this.state;

    return (
      <form onSubmit={this.onSubmit}>
        <div className="login-header">Sign up</div>

        <TextFieldGroup
          error={errors.username}
          label="Email Address"
          onChange={this.onChange}
          checkUserExists={this.checkUserExists}
          value={this.state.username}
          field="username"
        />
        <TextFieldGroup
          error={errors.password}
          label="Password"
          onChange={this.onChange}
          value={this.state.password}
          field="password"
          type="password"
        />

        <div style={{ color: "red" }} onChange={this.onChange}>
          {errors.passwordLength}
        </div>

        <TextFieldGroup
          error={errors.passwordConfirmation}
          label="Password Confirmation"
          onChange={this.onChange}
          value={this.state.passwordConfirmation}
          field="passwordConfirmation"
          type="password"
        />

        <div className="form-group login-button-div">
          <button
            disabled={this.state.isLoading || this.state.invalid}
            className="btn btn-primary btn-lg"
          >
            Sign up
          </button>
        </div>
        <div className="hyperlink-div">
          <a href="/login">Login</a>
        </div>
      </form>
    );
  }
}

SignupForm.propTypes = {
  signup: PropTypes.func.isRequired,
  addFlashMessage: PropTypes.func.isRequired,
  isUserExists: PropTypes.func.isRequired
};

SignupForm.contextTypes = {
  router: PropTypes.object.isRequired
};

export default withRouter(
  connect(
    null,
    { signup, isUserExists }
  )(SignupForm)
);
