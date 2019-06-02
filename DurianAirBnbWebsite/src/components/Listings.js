import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import {Button} from "primereact/button";
import ListingCard from "../components/ListingCard";
import ListingApi from "../api/ListingApi";
import GridList from "@material-ui/core/GridList";
import { MDBBtn } from "mdbreact";


class Listings extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      listings: [],
      guests: 2,
      hasMore: true,
      error: false,
      loading: false,
      usefilter: false,
      from: 0,
      count: 30
    };

    this.increment = this.increment.bind(this);
    this.decrement = this.decrement.bind(this);
    this.clearFilter = this.clearFilter.bind(this);
    this.applyFilter = this.applyFilter.bind(this);

    // Binds our scroll event handler
    window.onscroll = () => {
      const {
        loadListings,
        loadListingsWithFilter,
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
        if(this.state.usefilter)
          loadListingsWithFilter();
        else
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

  loadListingsWithFilter = () => {
    this.setState({ loading: true }, () => {
      ListingApi(this.state.from, this.state.count, this.state.guests).then(data => {
        this.setState({
          hasMore: this.state.listings.length < 10000,
          loading: false,
          from: this.state.from + this.state.count,
          listings: [...this.state.listings, ...data]
        });
      });
    });
  };

  decrement() {
    if(this.state.guests > 1) {
      this.setState({
        guests: this.state.guests - 1,
        message: null
      });
    } 
  }

  increment() {
    if(this.state.guests < 16) {
      this.setState({
        guests: this.state.guests + 1,
        message: null
      });
    } 
  }

  clearFilter() {
    this.setState({listings: [], from: 0, guests: 2, usefilter: false});
    this.loadListings();    
  }

  applyFilter() {
    this.setState({listings: [], from: 0, usefilter: true});
    this.loadListingsWithFilter();
  }
  
  componentDidMount() {
    this.loadListings();
  }


  render() {
    const { error, hasMore, loading } = this.state;
    const classes = this.props;


    return (
      <div className={classes.root}>
        <div className="inline-div">
        <div className="filter-div">
          <div className="filter-label-text">Guests</div>
            <div>
              <Button onClick={this.decrement} className="p-button-secondary" icon="pi pi-minus" />
              <span className="counter-text">{this.state.guests}</span>
              <Button onClick={this.increment} className="p-button-secondary" icon="pi pi-plus" />
            </div>
          </div>
          <div className="filter-apply"><MDBBtn rounded onClick={this.applyFilter}>APPLY</MDBBtn><MDBBtn flat onClick={this.clearFilter}>CLEAR</MDBBtn></div>
        </div>
        <GridList cellHeight={180} className={classes.gridList}>
          {this.state.listings.map(listing => (
            <ListingCard key={listing.Id} listing={listing} />
          ))}
        </GridList>
        {error && <div style={{ color: "#900" }}>{error}</div>}
        {loading && (
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
        )}
        {!hasMore && <div>You did it! You reached the end!</div>}
      </div>
    );
  }
}

Listings.propTypes = {
  auth: PropTypes.object.isRequired
};

function mapStateToProps(state) {
  return {
    auth: state.Auth
  };
}

export default connect(mapStateToProps)(Listings);
