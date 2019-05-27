import React from "react";
import PropTypes from "prop-types";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { login } from "../api/AuthActions";
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBInput,
  MDBBtn
} from "mdbreact";

class LoginForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: "",
      password: "",
      errors: {}
    };

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  isValid() {
    if (this.state.username.length < 1 || this.state.password.length < 1) {
      return false;
    } else {
      this.setState({ errors: {} });
      return true;
    }
  }

  handleInputChange = inputName => value => {
    if (inputName === "username") {
      this.setState({ username: value });
    }
    if (inputName === "password") {
      this.setState({ password: value });
    }
  };

  onSubmit(e) {
    e.preventDefault();
    if (this.isValid()) {
      this.setState({ errors: {} });
      this.props.login(this.state).then(
        res => this.props.history.push("/"),
        err =>
          this.setState({
            errors: err.response.data
          })
      );
    }
    e.target.className += " was-validated";
  }

  onChange(e) {
    this.setState({ [e.target.name]: e.target.value });
  }

  // Render the Login UI
  render() {
    let credentialError;
    if (this.state.errors.length > 0) {
      credentialError = this.state.errors;
    }

    return (
      <div className="bg cloudy-knoxville-gradient ">
      <div className="page-margin">
      <MDBContainer>
        <MDBRow className="justify-content-center">
          <MDBCol md="6">
            <MDBCard className="shadow-box-example z-depth-3">
              <div className="header pt-3 peach-gradient">
                <MDBRow className="d-flex justify-content-center">
                  <h3 className="white-text mb-3 pt-3 font-weight-bold">
                    LOG IN
                  </h3>
                </MDBRow>
              </div>
              <MDBCardBody className="mx-4 mt-4">
                {credentialError && (
                  <div className="alert alert-danger young-passion-gradient">
                    {credentialError}
                  </div>
                )}
                <form>
                  <MDBInput
                    value={this.state.username}
                    name="username"
                    label="Email Address"
                    group
                    type="email"
                    onChange={this.onChange}
                    //getValue={this.handleInputChange("username")}
                    validate
                    required
                  >
                    <div className="invalid-feedback">
                      Please use a valid email address.
                    </div>
                  </MDBInput>
                  <MDBInput
                    value={this.state.password}
                    label="Password"
                    group
                    type="password"
                    onChange={this.onChange}
                    validate
                    required
                    getValue={this.handleInputChange("password")}
                    containerClass="mb-0"
                  >
                    <div className="invalid-feedback">
                      Password cannot be empty.
                    </div>
                  </MDBInput>
                  <p className="font-small grey-text d-flex justify-content-end">
                    <a
                      href="#!"
                      className="dark-grey-text ml-1 font-weight-bold"
                    >
                      Forgot Password?
                    </a>
                  </p>
                  <MDBRow className="d-flex align-items-center mb-4 mt-5">
                    <MDBCol md="5" className="d-flex align-items-start">
                      <div className="text-center">
                        <MDBBtn
                          color="grey"
                          rounded
                          type="button"
                          className="z-depth-1a"
                          onClick={this.onSubmit}
                        >
                          Log in
                        </MDBBtn>
                      </div>
                    </MDBCol>
                    <MDBCol md="7" className="d-flex justify-content-end">
                      <p className="font-small grey-text mt-3">
                        Don't have an account?
                        <a
                          href="/Signup"
                          className="dark-grey-text ml-1 font-weight-bold"
                        >
                          Sign up
                        </a>
                      </p>
                    </MDBCol>
                  </MDBRow>
                </form>
              </MDBCardBody>
            </MDBCard>
          </MDBCol>
        </MDBRow>
      </MDBContainer>
      </div></div>
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
