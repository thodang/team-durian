import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import ListingCard from "../components/ListingCard";
import ListingApi from "../api/ListingApi";
import GridList from "@material-ui/core/GridList";

class Listings extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      listings: [],
      hasMore: true,
      error: false,
      loading: false,
      from: 0,
      count: 30
    };

    // Binds our scroll event handler
    window.onscroll = () => {
      const {
        loadListings,
        state: { error, loading, hasMore }
      } = this;

      // Bails early if:
      // * there's an error
      // * it's already loading
      // * there's nothing left to load
      if (error || loading || !hasMore) {
        return;
      }
      // Checks that the page has scrolled to the bottom
      if (window.innerHeight + window.scrollY >= document.body.scrollHeight) {
        loadListings();
      }
    };
  }

  loadListings = () => {
    this.setState({ loading: true }, () => {
      ListingApi(this.state.from, this.state.count).then(data => {
        // Merges the next listings into our existing listings
        this.setState({
          // Note: Depending on the API you're using, this value may
          // be returned as part of the payload to indicate that there
          // is no additional data to be loaded
          hasMore: this.state.listings.length < 10000,
          loading: false,
          from: this.state.from + this.state.count,
          listings: [...this.state.listings, ...data]
        });
      });
    });
  };

  componentDidMount() {
    this.loadListings();
  }

  render() {
    const { error, hasMore, loading } = this.state;
    const classes = this.props;

    return (
      <div className={classes.root}>
        <GridList cellHeight={180} className={classes.gridList}>
          {this.state.listings.map(listing => (
            <ListingCard key={listing.Id} listing={listing} />
          ))}
        </GridList>
        <hr />
        {error && <div style={{ color: "#900" }}>{error}</div>}
        {loading && (
          <div class="container-fluid">
            <div class="spinner-grow text-primary" role="status" />
            <div class="spinner-grow text-success" role="status" />
            <div class="spinner-grow text-danger" role="status" />
          </div>
        )}
        {!hasMore && <div>You did it! You reached the end!</div>}
      </div>
    );
  }
}

Listings.propTypes = {
  classes: PropTypes.object.isRequired,
  auth: PropTypes.object.isRequired
};

function mapStateToProps(state) {
  return {
    auth: state.Auth
  };
}

export default connect(mapStateToProps)(Listings);
