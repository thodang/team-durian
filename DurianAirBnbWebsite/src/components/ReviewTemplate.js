import React from "react";
import ShowMore from "react-show-more";

const ReviewTemplate = ({ review }) => (
  <div>
    <div>
      <div className="inline-div review-header review-info">
        {review.ReviewerName}{" "}
        <div className="host-id">(ID: {review.ReviewerId})</div>
      </div>
      <div className="review-date">{review.Date}</div>
    </div>
    <div className="section-info">
      <ShowMore
        lines={2}
        more=" Read more"
        less=" Hide"
        anchorClass="show-more"
      >
        {review.Comments}
      </ShowMore>
    </div>
    <hr className="hr-margin" />
  </div>
);

export default ReviewTemplate;
