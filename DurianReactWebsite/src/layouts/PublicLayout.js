import React from "react";
import Header from "../components/Header";

export const PublicLayout = ({ component: Component, children, ...rest }) => {
  return (
    <div>
      <Header {...rest} render={matchProps => <Component {...matchProps} />} />
      <div className="login-panel">
        <div className="main">{children}</div>
      </div>
    </div>
  );
};
