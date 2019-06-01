import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBInput,
  MDBBtn
} from "mdbreact";
import { resetPassword } from "../api/AuthActions";

class PasswordResetForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      password: "",
      passwordConfirmation: "",
      token: "",
      errors: {}
    };

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  isValid() {
    console.log("I'm validating");
    if (
      this.state.password.length < 1 ||
      this.state.passwordConfirmation.length < 1
    ) {
      return false;
    }
    if (this.state.password !== this.state.passwordConfirmation) {
      return this.setState({ errors: "passwords do not match" });
    } else {
      this.setState({ errors: {} });
      return true;
    }
  }

  handleInputChange = inputName => value => {
    console.log("inputName: " + inputName + "value: " + value);
    if (inputName === "passwordconfirm") {
      this.setState({ passwordConfirmation: value });
    }
    if (inputName === "password") {
      this.setState({ password: value });
    }
  };

  onSubmit(e) {
    e.preventDefault();
    if (this.isValid()) {
      this.setState(
        {
          errors: {},
          token: this.props.match.params.token
        },
        function() {
          this.props.resetPassword(this.state).then(
            () => {
              this.props.history.push("/login");
            },
            err => {
              this.setState({ errors: err.response.data });
            }
          );
        }
      );
    }
    e.target.className += " was-validated";
  }

  onChange(e) {
    this.setState({ [e.target.name]: e.target.value });
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
                        RESET PASSWORD
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
                              type="button"
                              className="z-depth-1a"
                              onClick={this.onSubmit}
                            >
                              Update Password
                            </MDBBtn>
                          </div>
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

PasswordResetForm.propTypes = {
  resetPassword: PropTypes.func.isRequired
};

PasswordResetForm.contextTypes = {
  router: PropTypes.object.isRequired
};

export default withRouter(
  connect(
    null,
    { resetPassword }
  )(PasswordResetForm)
);
