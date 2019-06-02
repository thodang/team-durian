import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import ShowMore from "react-show-more";
import { Rating } from "primereact/rating";
import ListingDetailApi from "../api/ListingDetailApi";
import ReviewTemplate from "../components/ReviewTemplate";
import Pagination from "jw-react-pagination";
import { MDBIcon } from "mdbreact";

class ListingDetail extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      listing: {},
      reviews: [],
      pageOfItems: [],
      error: false,
      loading: true
    };
    this.onChangePage = this.onChangePage.bind(this);
  }

  onChangePage(pageOfItems) {
    this.setState({ pageOfItems: pageOfItems });
  }

  async componentDidMount() {
    this._asyncEventsRequest = await ListingDetailApi(
      this.props.match.params.listingId
    ).then(data => {
      this.setState({ listing: data, reviews: data.Reviews, loading: false });
    });
  }

  render() {
    const { listing, reviews, loading } = this.state;

    if (loading) {
      return (
        <div className="center-flex">
          <div
            className="spinner-grow text-primary spinner-spacing"
            role="status"
          />
          <div
            className="spinner-grow text-success spinner-spacing"
            role="status"
          />
          <div
            className="spinner-grow text-danger spinner-spacing"
            role="status"
          />
        </div>
      );
    }

    return (
      <div>
        <div>
          <img className="listing-image" src={listing.PictureUrl} alt="..." />
        </div>
        <div className="container">
          <div className="title-header inline-div">
            <div className="left-div">{listing.Name}</div>{" "}
            <div className="right-div">
              <img
                src={listing.HostThumbnailUrl}
                alt=""
                className="rounded-circle z-depth-1-half"
              />
            </div>
          </div>
          <div className="inline-div">
            <div className="section-info left-div">
              {listing.City}, {listing.State}
            </div>{" "}
            <div className="right-div host-info">{listing.HostName}</div>
          </div>
          <div className="inline-div">
            <div className="section-info-bold">{listing.Price}</div>{" "}
            <div className="section-info-no-pad">per night</div>
          </div>

          <hr className="hr-margin" />

          <div className="section-header">Summary</div>
          <div className="section-info">{listing.Summary}</div>

          <hr className="hr-margin" />

          <div className="section-header">Description</div>
          <div className="section-info">
            <ShowMore
              lines={2}
              more=" Read more about the space"
              less=" Hide"
              anchorClass="show-more"
            >
              {listing.Description}
            </ShowMore>
          </div>

          <hr className="hr-margin" />

          <div className="section-header">Amenities</div>
          <div className="section-info">
            <MDBIcon icon="bed" /> Bedrooms: {listing.Bedrooms}
          </div>
          <div className="section-info">
            <MDBIcon icon="bath" /> Bathrooms: {listing.Bathrooms}
          </div>

          <hr className="hr-margin" />

          <div className="inline-div section-header">
            {listing.NumberOfReviews} Reviews &nbsp;&nbsp;&nbsp;
            <div className="review-negative-margin">
              <Rating
                value={(listing.ReviewScoresRating * 5) / 100}
                readonly={true}
                stars={5}
                cancel={false}
              />
            </div>
          </div>

          <div>
            {this.state.pageOfItems.map(review => (
              <ReviewTemplate key={review.Id} review={review} />
            ))}
            <Pagination
              items={reviews}
              onChangePage={this.onChangePage}
              pageSize={5}
              disableDefaultStyles={true}
            />
          </div>
        </div>
      </div>
    );
  }
}

ListingDetail.propTypes = {
  auth: PropTypes.object.isRequired
};

function mapStateToProps(state) {
  return {
    auth: state.Auth
  };
}

export default connect(mapStateToProps)(ListingDetail);
