import React from "react";
import Header from "../components/Header";

export const PrivateLayout = ({ component: Component, children, ...rest }) => {
  return (
    <div>
      {}
      <div className="wrapper">
        <div className="close-layer" />
        <div>
          <Header
            {...rest}
            render={matchProps => <Component {...matchProps} />}
          />
        </div>
        <div className="main-panel">
          {children}
          <div className="bottom-spacing" />
        </div>
      </div>
    </div>
  );
};
