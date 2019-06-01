import React from "react";
import PropTypes from "prop-types";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { requestPasswordReset } from "../api/AuthActions";
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBInput,
  MDBBtn
} from "mdbreact";

class ForgotPasswordForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: "",
      errors: {},
      responseMessage: "",
      isLoading: false
    };

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  isValid() {
    if (this.state.username.length < 1) {
      return false;
    } else {
      this.setState({ errors: {} });
      return true;
    }
  }

  onSubmit(e) {
    e.preventDefault();
    if (this.isValid()) {
      this.setState({ errors: {}, isLoading: true });
      this.props.requestPasswordReset(this.state).then(
        res => {
          this.setState({
            responseMessage:
              "Please check your email for password reset instructions."
          });
        },
        err =>
          this.setState({
            errors: err.response.data
          })
      );
    }
    e.target.className += " was-validated";
  }

  handleInputChange = inputName => value => {
    if (inputName === "username") {
      this.setState({ username: value });
    }
  };

  onChange(e) {
    this.setState({ [e.target.name]: e.target.value });
  }

  render() {
    let confirmationMessage, credentialError;

    if (this.state.errors.length > 0) {
      credentialError = this.state.errors;
    }
    if (this.state.responseMessage.length > 0) {
      confirmationMessage = this.state.responseMessage;
    }

    return (
      <div className="bg cloudy-knoxville-gradient ">
        <div className="page-margin" />
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
                        {confirmationMessage && (
                          <div className="password-reset">
                            {confirmationMessage}
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
                      validate
                      required
                    >
                      <div className="invalid-feedback">
                        Please use a valid email address.
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
                          >
                            RESET
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
    );
  }
}

ForgotPasswordForm.propTypes = {
  requestPasswordReset: PropTypes.func.isRequired
};
/*
ForgotPasswordForm.contextTypes = {
  router: PropTypes.object.isRequired
};
*/
export default withRouter(
  connect(
    null,
    { requestPasswordReset }
  )(ForgotPasswordForm)
);
