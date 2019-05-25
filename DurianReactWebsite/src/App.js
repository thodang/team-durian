import React, { Component } from "react";
import "./App.css";
import Home from "./components/Home";
import Books from "./components/Books";
import Login from "./components/login";
import Signup from "./components/Signup";
import OrderFulfillment from "./components/OrderFulfillment";
import { PrivateRoute, PublicRoute } from "../src/Routes/index";
import requireAuth from "../src/Utils/RequireAuth";
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
                <Redirect to="/home" />
              </Route>
              <PublicRoute path="/login" component={Login} />
              <PublicRoute path="/signup" component={Signup} />
              <PublicRoute path="/home" component={Home} />
              <PrivateRoute path="/books" component={Books} />
              <PrivateRoute
                path="/orderfulfillment"
                component={requireAuth(OrderFulfillment)}
              />
            </Switch>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
