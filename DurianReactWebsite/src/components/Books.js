import React from "react";
import axios from "axios";
import { connect } from "react-redux";
import classNames from "classnames";
import PropTypes from "prop-types";
import { withStyles } from "@material-ui/core/styles";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TablePagination from "@material-ui/core/TablePagination";
import TableRow from "@material-ui/core/TableRow";
import TableSortLabel from "@material-ui/core/TableSortLabel";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Paper from "@material-ui/core/Paper";
import Checkbox from "@material-ui/core/Checkbox";
import IconButton from "@material-ui/core/IconButton";
import Tooltip from "@material-ui/core/Tooltip";
import DeleteIcon from "@material-ui/icons/Delete";
import FilterListIcon from "@material-ui/icons/FilterList";
import { lighten } from "@material-ui/core/styles/colorManipulator";
import Button from "@material-ui/core/Button";
import BookApi from "../api/BookApi";
import { logout } from "../api/AuthActions";

function desc(a, b, orderBy) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

function stableSort(array, cmp) {
  const stabilizedThis = array.map((el, index) => [el, index]);
  stabilizedThis.sort((a, b) => {
    const order = cmp(a[0], b[0]);
    if (order !== 0) return order;
    return a[1] - b[1];
  });
  return stabilizedThis.map(el => el[0]);
}

function getSorting(order, orderBy) {
  return order === "desc"
    ? (a, b) => desc(a, b, orderBy)
    : (a, b) => -desc(a, b, orderBy);
}

const rows = [
  { id: "isbn", numeric: false, disablePadding: true, label: "ISBN" },
  { id: "title", numeric: false, disablePadding: false, label: "Title" },
  { id: "authors", numeric: false, disablePadding: false, label: "Authors" },
  {
    id: "available_inventory",
    numeric: true,
    disablePadding: false,
    label: "# Available"
  },
  { id: "price", numeric: true, disablePadding: false, label: "Price ($)" }
];

class EnhancedTableHead extends React.Component {
  createSortHandler = property => event => {
    this.props.onRequestSort(event, property);
  };

  render() {
    const {
      onSelectAllClick,
      order,
      orderBy,
      numSelected,
      rowCount
    } = this.props;

    return (
      <TableHead>
        <TableRow>
          <TableCell padding="checkbox">
            <Checkbox
              indeterminate={numSelected > 0 && numSelected < rowCount}
              checked={numSelected === rowCount}
              onChange={onSelectAllClick}
            />
          </TableCell>
          {rows.map(
            row => (
              <TableCell
                key={row.id}
                align={row.numeric ? "right" : "left"}
                padding={row.disablePadding ? "none" : "default"}
                sortDirection={orderBy === row.id ? order : false}
              >
                <Tooltip
                  title="Sort"
                  placement={row.numeric ? "bottom-end" : "bottom-start"}
                  enterDelay={300}
                >
                  <TableSortLabel
                    active={orderBy === row.id}
                    direction={order}
                    onClick={this.createSortHandler(row.id)}
                  >
                    {row.label}
                  </TableSortLabel>
                </Tooltip>
              </TableCell>
            ),
            this
          )}
        </TableRow>
      </TableHead>
    );
  }
}

EnhancedTableHead.propTypes = {
  numSelected: PropTypes.number.isRequired,
  onRequestSort: PropTypes.func.isRequired,
  onSelectAllClick: PropTypes.func.isRequired,
  order: PropTypes.string.isRequired,
  orderBy: PropTypes.string.isRequired,
  rowCount: PropTypes.number.isRequired
};

const toolbarStyles = theme => ({
  root: {
    paddingRight: theme.spacing.unit
  },
  highlight:
    theme.palette.type === "light"
      ? {
          color: theme.palette.secondary.main,
          backgroundColor: lighten(theme.palette.secondary.light, 0.85)
        }
      : {
          color: theme.palette.text.primary,
          backgroundColor: theme.palette.secondary.dark
        },
  spacer: {
    flex: "1 1 100%"
  },
  actions: {
    color: theme.palette.text.secondary
  },
  title: {
    flex: "0 0 auto"
  }
});

let EnhancedTableToolbar = props => {
  const { numSelected, classes } = props;

  return (
    <div>
      <span className="table-header">Books Collection</span>
      <Toolbar
        className={classNames(classes.root, {
          [classes.highlight]: numSelected > 0
        })}
      >
        <div className={classes.title}>
          {numSelected > 0 ? (
            <Typography color="inherit" variant="subtitle1">
              {numSelected} selected
            </Typography>
          ) : (
            <Typography variant="h6" id="tableTitle" />
          )}
        </div>
        <div className={classes.spacer} />
        <div className={classes.actions}>
          {numSelected > 0 ? (
            <Tooltip title="Delete">
              <IconButton aria-label="Delete">
                <DeleteIcon />
              </IconButton>
            </Tooltip>
          ) : (
            <Tooltip title="Filter list">
              <IconButton aria-label="Filter list">
                <FilterListIcon />
              </IconButton>
            </Tooltip>
          )}
        </div>
      </Toolbar>
    </div>
  );
};

EnhancedTableToolbar.propTypes = {
  classes: PropTypes.object.isRequired,
  numSelected: PropTypes.number.isRequired
};

EnhancedTableToolbar = withStyles(toolbarStyles)(EnhancedTableToolbar);

const styles = theme => ({
  root: {
    width: "100%",
    marginTop: theme.spacing.unit * 3
  },
  table: {
    minWidth: 1020
  },
  tableWrapper: {
    overflowX: "auto"
  }
});

class Books extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      order: "asc",
      orderBy: "calories",
      selected: [],
      books: [],
      page: 0,
      rowsPerPage: 10,
      orderId: ""
    };

    const emptyRows =
      this.state.rowsPerPage -
      Math.min(
        this.state.rowsPerPage,
        this.state.books.length - this.state.page * this.state.rowsPerPage
      );
  }

  submitNewOrder(list) {
    const data = {
      customer_id: "5ccf16a5cb81384ed03c0154",
      book_id: list
    };

    axios
      .post("https://cmpe-durian.azurewebsites.net/api/order", data)
      .then(res => {
        this.setState({ orderId: 'Order "' + res.data + '" created' });
      });
    this.setState({ selected: [] });
  }

  async componentDidMount() {
    this._asyncEventsRequest = await BookApi().then(data => {
      this.setState({ books: data });
    });
  }

  handleRequestSort = (event, property) => {
    const orderBy = property;
    let order = "desc";

    if (this.state.orderBy === property && this.state.order === "desc") {
      order = "asc";
    }

    this.setState({ order, orderBy });
  };

  handleSelectAllClick = event => {
    if (event.target.checked) {
      this.setState(state => ({ selected: state.books.map(n => n.Id) }));
      return;
    }
    this.setState({ selected: [] });
  };

  handleClick = (event, id) => {
    const { selected } = this.state;
    const selectedIndex = selected.indexOf(id);
    let newSelected = [];

    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, id);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1)
      );
    }

    this.setState({ selected: newSelected });
  };

  handleChangePage = (event, page) => {
    this.setState({ page });
  };

  handleChangeRowsPerPage = event => {
    this.setState({ rowsPerPage: event.target.value });
  };

  isSelected = id => this.state.selected.indexOf(id) !== -1;

  renderReadOnlyTable() {
    return (
      <Paper className={this.props.classes.root}>
        <EnhancedTableToolbar numSelected={this.state.selected.length} />
        <div className={this.props.classes.tableWrapper}></div>
        <Table className={this.props.classes.table}>
          <TableHead>
            <TableRow>
              <TableCell align="left">ISBN</TableCell>
              <TableCell align="left">Title</TableCell>
              <TableCell align="left">Authors</TableCell>
              <TableCell align="right">Available #</TableCell>
              <TableCell align="right">Price ($)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {stableSort(
              this.state.books,
              getSorting(this.state.order, this.state.orderBy)
            )
              .slice(
                this.state.page * this.state.rowsPerPage,
                this.state.page * this.state.rowsPerPage +
                  this.state.rowsPerPage
              )
              .map(book => (
                <TableRow key={book.Id}>
                  <TableCell component="th" scope="row">
                    {book.isbn}
                  </TableCell>
                  <TableCell align="left">{book.title}</TableCell>
                  <TableCell align="left">{book.authors}</TableCell>
                  <TableCell align="right">
                    {book.available_inventory}
                  </TableCell>
                  <TableCell align="right">{book.price}</TableCell>
                </TableRow>
              ))}
          </TableBody>
        </Table>

        <TablePagination
          rowsPerPageOptions={[5, 10, 25]}
          component="div"
          count={this.state.books.length}
          rowsPerPage={this.state.rowsPerPage}
          page={this.state.page}
          backIconButtonProps={{
            "aria-label": "Previous Page"
          }}
          nextIconButtonProps={{
            "aria-label": "Next Page"
          }}
          onChangePage={this.handleChangePage}
          onChangeRowsPerPage={this.handleChangeRowsPerPage}
        />
      </Paper>
    );
  }

  renderActiveTable() {
    return (
      <div>
        <Paper className={this.props.classes.root}>
          <EnhancedTableToolbar numSelected={this.state.selected.length} />
          <div className={this.props.classes.tableWrapper}>
            <Table
              className={this.props.classes.table}
              aria-labelledby="tableTitle"
            >
              <EnhancedTableHead
                numSelected={this.state.selected.length}
                order={this.state.order}
                orderBy={this.state.orderBy}
                onSelectAllClick={this.handleSelectAllClick}
                onRequestSort={this.handleRequestSort}
                rowCount={this.state.books.length}
              />
              <TableBody>
                {stableSort(
                  this.state.books,
                  getSorting(this.state.order, this.state.orderBy)
                )
                  .slice(
                    this.state.page * this.state.rowsPerPage,
                    this.state.page * this.state.rowsPerPage +
                      this.state.rowsPerPage
                  )
                  .map(n => {
                    const isSelected = this.isSelected(n.Id);
                    return (
                      <TableRow
                        hover
                        onClick={event => this.handleClick(event, n.Id)}
                        role="checkbox"
                        aria-checked={isSelected}
                        tabIndex={-1}
                        key={n.Id}
                        selected={isSelected}
                      >
                        <TableCell padding="checkbox">
                          <Checkbox checked={isSelected} />
                        </TableCell>
                        <TableCell component="th" scope="row" padding="none">
                          {n.isbn}
                        </TableCell>
                        <TableCell align="left">{n.title}</TableCell>
                        <TableCell align="left">{n.authors}</TableCell>
                        <TableCell align="right">
                          {n.available_inventory}
                        </TableCell>
                        <TableCell align="right">{n.price}</TableCell>
                      </TableRow>
                    );
                  })}
                {this.emptyRows > 0 && (
                  <TableRow style={{ height: 49 * this.emptyRows }}>
                    <TableCell colSpan={6} />
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </div>
          <TablePagination
            rowsPerPageOptions={[5, 10, 25]}
            component="div"
            count={this.state.books.length}
            rowsPerPage={this.state.rowsPerPage}
            page={this.state.page}
            backIconButtonProps={{
              "aria-label": "Previous Page"
            }}
            nextIconButtonProps={{
              "aria-label": "Next Page"
            }}
            onChangePage={this.handleChangePage}
            onChangeRowsPerPage={this.handleChangeRowsPerPage}
          />
        </Paper>
        <div className="primary-button-div">
          <Button
            variant="contained"
            color="primary"
            className={this.props.classes.button}
            onClick={() => this.submitNewOrder(this.state.selected)}
          >
            Submit Order
          </Button>
          <span className="response-text">{this.state.orderId}</span>
        </div>
      </div>
    );
  }

  render() {
    if (
      this.props.auth.user === null ||
      JSON.stringify(this.props.auth.user) === "{}"
    ) {
      return <div>{this.renderReadOnlyTable()}</div>;
    } else {
      return <div>{this.renderActiveTable()}</div>;
    }
  }
}

Books.propTypes = {
  classes: PropTypes.object.isRequired,
  auth: PropTypes.object.isRequired,
  logout: PropTypes.func.isRequired
};

function mapStateToProps(state) {
  return {
    auth: state.Auth
  };
}

export default connect(
  mapStateToProps,
  { logout }
)(withStyles(styles)(Books));
