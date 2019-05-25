import React from "react";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import PropTypes from "prop-types";
import { addFlashMessage } from "../actions/flashMessages";

export default function(ComposedComponent) {
  class Authenticate extends React.Component {
    componentWillMount() {
      if (!this.props.isAuthenticated) {
        this.props.addFlashMessage({
          type: "error",
          text: "You need to login to access this page"
        });
        this.props.history.push("/login");
      }
    }

    componentWillUpdate(nextProps) {
      if (!nextProps.isAuthenticated) {
        this.props.history.push("/");
      }
    }

    render() {
      return <ComposedComponent {...this.props} />;
    }
  }

  Authenticate.propTypes = {
    isAuthenticated: PropTypes.bool.isRequired,
    addFlashMessage: PropTypes.func.isRequired
  };

  function mapStateToProps(state) {
    return {
      isAuthenticated: state.Auth.isAuthenticated,
      user: state.Auth.user,
      userId: state.Auth.userId
    };
  }

  return withRouter(
    connect(
      mapStateToProps,
      { addFlashMessage }
    )(Authenticate)
  );
}
