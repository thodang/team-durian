import React from "react";

class Header extends React.Component {
  /*
  logout(e) {
    e.preventDefault();
    this.props.logout();
  }
*/
  render() {
    //let user = this.props.auth.user.toString();

    return (
      <div id="app" class="container-fluid">
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
          <span className="page-title-header">Durian Bookstore</span>

          {/*
          <button
            class="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target="#navbarNavDropdown"
            aria-controls="navbarNavDropdown"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span class="navbar-toggler-icon" />
          </button>
          */}
          <div id="navbarNavDropdown" class="navbar-collapse collapse">
            <ul class="navbar-nav mr-auto">
              <li class="nav-item">
                <a class="nav-link" href="/home">
                  Home
                </a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/books">
                  Books
                </a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/orderfulfillment">
                  Order Fulfillment
                </a>
              </li>
              {/*
              <li class="nav-item">
                <a class="nav-link" href="#">
                  Pricing
                </a>
              </li>
              */}
            </ul>
            <ul class="navbar-nav">
              {/*<li class="nav-item dropdown">
                <a
                  class="nav-link dropdown-toggle"
                  href="http://example.com"
                  id="navbarDropdownMenuLink"
                  data-toggle="dropdown"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  Dropdown
                </a>
                <div
                  class="dropdown-menu"
                  aria-labelledby="navbarDropdownMenuLink"
                >
                  <a class="dropdown-item" href="#">
                    Action
                  </a>
                  <a class="dropdown-item" href="#">
                    Another action
                  </a>
                </div>
              </li>
            */}
              <li class="nav-item">
                <span className="nav-text">Graciela Ruta</span>
              </li>
              {/*<li class="nav-item">
                <a class="nav-link" href="{{ url('/register') }}">
                  Register
                </a>
              </li>
                */}
            </ul>
          </div>
        </nav>
      </div>
    );
  }
}

export default Header;
