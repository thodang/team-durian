import React from "react";
import { connect } from "react-redux";
import { Nav, Navbar } from "react-bootstrap";
import { logout } from "../api/AuthActions";
import PropTypes from "prop-types";

class Header extends React.Component {
  logout(e) {
    e.preventDefault();
    this.props.logout();
  }

  renderLogin() {
    if (
      this.props.auth.user === null ||
      JSON.stringify(this.props.auth.user) === "{}"
    ) {
      return <Nav.Link href="/login">Login</Nav.Link>;
    } else {
      return (
        <Nav>
          <Navbar.Brand>{this.props.auth.user.toString()}</Navbar.Brand>
          <Nav.Link href="/login" onClick={this.logout.bind(this)}>
            Logout
          </Nav.Link>
        </Nav>
      );
    }
  }

  render() {
    return (
      <Navbar bg="light" expand="lg">
        <Navbar.Brand>Durian Online Bookstore</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mr-auto">
            <Nav.Link href="/home">Home</Nav.Link>
            <Nav.Link href="/books">Books</Nav.Link>
            <Nav.Link href="/orderfulfillment">Order Fulfillment</Nav.Link>
          </Nav>
          <Nav>{this.renderLogin()}</Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}

Header.propTypes = {
  auth: PropTypes.object.isRequired,
  logout: PropTypes.func.isRequired
};

function mapStateToProps(state) {
  return {
    auth: state.Auth
  };
}

export default connect(
  mapStateToProps,
  { logout }
)(Header);
