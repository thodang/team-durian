import React from "react";
import { Route } from "react-router-dom";
import { PublicLayout } from "../layouts";

export const PublicRoute = ({ component: Component, ...rest }) => {
  return (
    <Route
      {...rest}
      render={matchProps => (
        <PublicLayout>
          <Component {...matchProps} />
        </PublicLayout>
      )}
    />
  );
};
