import React from "react";
import ReactDOM from "react-dom";
import { HashRouter } from "react-router-dom";
import "./Styles/base.scss";
import setAuthorizationToken from "../src/Utils/setAuthorizationToken";
import configureStore from "./config/configureStore";
import { Provider } from "react-redux";
import {
  setCurrentUser,
  getUserNameFromToken,
  getUserIdFromToken
} from "../src/api/AuthActions";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "bootstrap-css-only/css/bootstrap.min.css";
import "mdbreact/dist/css/mdb.css";
import "primereact/resources/themes/nova-light/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";

const store = configureStore();
const rootElement = document.getElementById("root");

if (localStorage.durianToken) {
  store.dispatch(
    setCurrentUser(
      getUserNameFromToken(localStorage.durianToken),
      getUserIdFromToken(localStorage.durianToken)
    )
  );
}

const renderApp = Component => {
  ReactDOM.render(
    <Provider store={store}>
      <HashRouter>
        <Component />
      </HashRouter>
    </Provider>,
    rootElement
  );
};

renderApp(App);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
