import React from "react";
import { Link } from "react-router-dom";
import {
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCardTitle,
  MDBCardText,
  MDBCol
} from "mdbreact";
import { Rating } from "primereact/rating";

const ListingCard = ({ listing }) => (
  <div className="card-spacing">
    <Link to={`/listingdetail/${listing.ListingId}`} className="link-style">
      <MDBCol>
        <MDBCard style={{ width: "25rem", height: "32rem" }}>
          <MDBCardImage
            style={{ height: "18rem" }}
            className="img-fluid"
            src={listing.PictureUrl}
          />
          <MDBCardBody style={{ height: "10rem" }}>
            <MDBCardText>{listing.Name.substring(0, 40) + "..."}</MDBCardText>
            <MDBCardTitle>
              {listing.Summary.substring(0, 50) + "..."}
            </MDBCardTitle>
            <MDBCardText>{listing.Price} per night <span className="right-span">{listing.Accommodates} guests</span></MDBCardText>
            <div className="inline-div content-section fixed-review">
              <Rating
                value={(listing.ReviewScoresRating * 5) / 100}
                readonly={true}
                stars={5}
                cancel={false}
              />
              &nbsp;&nbsp;&nbsp;{listing.NumberOfReviews}
            </div>
          </MDBCardBody>
        </MDBCard>
      </MDBCol>
    </Link>
  </div>
);

export default ListingCard;
