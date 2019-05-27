import React, { Component } from "react";
import "./App.css";
import Listings from "./components/Listings";
import Login from "./components/login";
import Signup from "./components/Signup";
import { PrivateRoute, PublicRoute } from "../src/Routes/index";
import requireAuth from "../src/Utils/RequireAuth";
import ListingDetail from "./components/ListingDetail";
import {
  Route,
  BrowserRouter as Router,
  Switch,
  Redirect
} from "react-router-dom";

class App extends Component {
  render() {
    return (
      <div>
        <Router>
          <div>
            <Switch>
              <Route exact path="/">
                <Redirect to="/listings" />
              </Route>
              <PublicRoute path="/login" component={Login} />
              <PublicRoute path="/signup" component={Signup} />
              <PrivateRoute
                path="/listings"
                component={requireAuth(Listings)}
              />
              <PrivateRoute
                path="/listingdetail/:listingId"
                component={requireAuth(ListingDetail)}
              />
            </Switch>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
