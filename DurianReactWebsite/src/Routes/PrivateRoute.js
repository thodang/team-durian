import React from "react";
import { PrivateLayout } from "../layouts";
import { Route } from "react-router-dom";

export const PrivateRoute = ({ component: Component, ...rest }) => {
  return (
    <Route
      {...rest}
      render={matchProps => (
        <PrivateLayout {...matchProps}>
          <Component />
        </PrivateLayout>
      )}
    />
  );
};
