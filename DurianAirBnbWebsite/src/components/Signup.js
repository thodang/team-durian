import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import { signup, isUserExists } from "../api/AuthActions";
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBInput,
  MDBBtn
} from "mdbreact";

class SignupForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: "",
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

  handleInputChange = inputName => value => {
    if (inputName === "username") {
      this.setState({ username: value });
    }
    if (inputName === "password") {
      this.setState({ password: value });
    }
    if (inputName === "passwordconfirm") {
      this.setState({ passwordConfirmation: value });
    }
  };

  onChange(e) {
    this.setState({ [e.target.name]: e.target.value });
  }

  isValid() {
    if (
      this.state.username.length < 1 ||
      this.state.password.length < 1 ||
      this.state.passwordConfirmation.length < 1
    ) {
      return false;
    }
    if (this.state.password !== this.state.passwordConfirmation) {
      this.setState({ errors: "please confirm correct password" });
      return false;
    } else {
      this.setState({ errors: {} });
      return true;
    }
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
    e.target.className += " was-validated";
  }

  onSubmit(e) {
    e.preventDefault();
    if (this.isValid()) {
      this.setState({ errors: {} });
      this.props.signup(this.state).then(
        () => {
          this.props.history.push("/");
        },
        err => {
          this.setState({ errors: err.response.data });
        }
      );
    }
    e.target.className += " was-validated";
  }

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
                        SIGN UP
                      </h3>
                    </MDBRow>
                  </div>
                  <MDBCardBody className="mx-4 mt-4">
                    <form
                      className="needs-validation"
                      onSubmit={this.onSubmit}
                      noValidate
                    >
                      <div>
                        <MDBRow>
                          {credentialError && (
                            <div className="col-sm-12 alert alert-danger young-passion-gradient">
                              {credentialError}
                            </div>
                          )}
                        </MDBRow>
                      </div>
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

                      <MDBInput
                        value={this.state.passwordConfirmation}
                        label="Confirm password"
                        group
                        type="password"
                        validate
                        required
                        onChange={this.onChange}
                        getValue={this.handleInputChange("passwordconfirm")}
                        containerClass="mb-0"
                      >
                        <div className="invalid-feedback">
                          Password cannot be empty.
                        </div>
                      </MDBInput>

                      <MDBRow className="d-flex align-items-center mb-4 mt-5">
                        <MDBCol md="5" className="d-flex align-items-start">
                          <div className="text-center">
                            <MDBBtn
                              color="grey"
                              rounded
                              type="submit"
                              className="z-depth-1a"
                              //onClick={this.onSubmit}
                            >
                              Sign Up
                            </MDBBtn>
                          </div>
                        </MDBCol>
                        <MDBCol md="7" className="d-flex justify-content-end">
                          <p className="font-small grey-text mt-3">
                            Already have an account?
                            <a
                              href="/login"
                              className="dark-grey-text ml-1 font-weight-bold"
                            >
                              Login
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
        </div>
      </div>
    );
  }
}

SignupForm.propTypes = {
  signup: PropTypes.func.isRequired,
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
