import React from "react";

export const PublicLayout = ({ component: Component, children, ...rest }) => {
  return (
    <div>
      <div className="login-panel">
        <div className="main">{children}</div>
      </div>
    </div>
  );
};
