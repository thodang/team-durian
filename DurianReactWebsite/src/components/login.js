import React from "react";
import PropTypes from "prop-types";
import { withRouter } from "react-router-dom";
import TextFieldGroup from "../components/TextFieldGroup";
import validateInput from "../Validation/validateInputLogin";
import { connect } from "react-redux";
import { login } from "../api/AuthActions";

class LoginForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: "",
      password: "",
      errors: {},
      isLoading: false
    };

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  isValid() {
    const { errors, isValid } = validateInput(this.state);

    if (!isValid) {
      this.setState({ errors });
    }

    return isValid;
  }

  onSubmit(e) {
    e.preventDefault();
    if (this.isValid()) {
      this.setState({ errors: {}, isLoading: true });
      this.props.login(this.state).then(
        res => this.props.history.push("/books"),
        err =>
          this.setState({
            errors: err.response.data,
            isLoading: false
          })
      );
    }
  }

  onChange(e) {
    this.setState({ [e.target.name]: e.target.value });
  }

  // Render the Login UI
  render() {
    const { errors, username, password, isLoading } = this.state;
    let credentialError;
    if (this.state.errors.length > 0) {
      credentialError = this.state.errors;
    }
    return (
      <form onSubmit={this.onSubmit}>
        <div className="login-header">Login</div>

        {credentialError && (
          <div className="alert alert-danger">{credentialError}</div>
        )}
        <TextFieldGroup
          field="username"
          label="Email Address"
          value={username}
          error={errors.username}
          onChange={this.onChange}
        />
        <TextFieldGroup
          field="password"
          label="Password"
          value={password}
          error={errors.password}
          onChange={this.onChange}
          type="password"
        />
        <div className="form-group login-button-div">
          <button className="btn btn-primary btn-lg" disabled={isLoading}>
            Login
          </button>
        </div>
        <div className="hyperlink-div">
          <a href="/signup">Sign up</a>
        </div>
      </form>
    );
  }
}

LoginForm.propTypes = {
  login: PropTypes.func.isRequired
};

export default withRouter(
  connect(
    null,
    { login }
  )(LoginForm)
);
