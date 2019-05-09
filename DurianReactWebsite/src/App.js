import React, { Component } from "react";
import "./App.css";
import Home from "./components/Home";
import Books from "./components/Books";
import Header from "./components/Header";
import OrderFulfillment from "./components/OrderFulfillment";
//import { PrivateRoute } from "./components/PrivateRoute";
import { BrowserRouter, Route } from "react-router-dom";

class App extends Component {
  render() {
    return (
      <div>
        <Header />

        <BrowserRouter>
          <div className="container-fluid">
            <Route exact={true} path="/" component={Home} />
            <Route exact={true} path="/home" component={Home} />
            <Route exact={true} path="/books" component={Books} />
            <Route
              exact={true}
              path="/orderfulfillment"
              component={OrderFulfillment}
            />
            {/*
          <PrivateRoute exact={true} path="/account" component={Account}/>          
          <PrivateRoute exact={true} path="/orders" component={Orders}/>
          <PrivateRoute exact={true} path="/cart" component={Cart}/>
          <PrivateRoute exact={true} path="/search" component={Search}/>
          <PrivateRoute exact={true} path="/checkout" component={Checkout}/>
          <Route exact={true} path="/login" render={Login}/>
          <Route exact={true} path="/register" render={Registration}/>
          <Route exact={true} path="/signout" render={SignOut}/>
          */}
          </div>
        </BrowserRouter>
      </div>
    );
  }
}

export default App;
