import React from "react";

const ReviewTemplate = ({ review }) => (
    <div>
      <div>
        <div className="inline-div review-header">{review.ReviewerName} <div className="host-id">(ID: {review.ReviewerId})</div></div>
        <div className="review-date">{review.Date}</div>
      </div>
      <div className="section-info">{review.Comments}</div>
      <hr className="hr-margin"/>
    </div>
);

export default ReviewTemplate;
