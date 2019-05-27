import React from "react";
import { connect } from "react-redux";
import { Nav, Navbar } from "react-bootstrap";
import { logout } from "../api/AuthActions";
import PropTypes from "prop-types";
import {
  MDBNavbar,
  MDBNavbarBrand,
  MDBNavbarNav,
  MDBNavItem,
  MDBCollapse
} from "mdbreact";

class Header extends React.Component {
  state = {
    isOpen: false
  };

  toggleCollapse = () => {
    this.setState({ isOpen: !this.state.isOpen });
  };

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
          <Nav.Link
            className="navbar-item-text"
            href="/login"
            onClick={this.logout.bind(this)}
          >
            Logout
          </Nav.Link>
        </Nav>
      );
    }
  }

  render() {
    return (
      <MDBNavbar color="young-passion-gradient" dark expand="md">
        <MDBNavbarBrand>
          <strong className="navbar-title-text">Durian Online Listings</strong>
        </MDBNavbarBrand>
        <MDBCollapse id="navbarCollapse3" isOpen={this.state.isOpen} navbar>
          <MDBNavbarNav right>
            <MDBNavItem>
              <Nav>{this.renderLogin()}</Nav>
            </MDBNavItem>
          </MDBNavbarNav>
        </MDBCollapse>
      </MDBNavbar>
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
