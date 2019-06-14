import React from 'react';
import { connect } from 'react-redux';
import { Button, ButtonGroup, Table } from 'reactstrap'
import _ from 'lodash';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faTimes } from '@fortawesome/free-solid-svg-icons';
import NewEnvironmentForm from './../components/NewEnvironmentForm';
import { addNewEnvironment, deleteEnvironment, getAllEnvironments } from './../actions/environments';

class EnvironmentsView extends React.Component {
  componentDidMount() {
    this.props.getAllEnvironments();
  }

  render() {
    let tableContent;
    if (!this.props.availableEnvironments || this.props.availableEnvironments.length === 0) {
      tableContent = (
        <tr>
          <td colSpan="2">
            <span>No Environments. Click the <FontAwesomeIcon icon={faPlus} /> icon to add a new one.</span>
          </td>
        </tr>
      );
    } else {
      tableContent = _.map(this.props.availableEnvironments, e => (
        <tr key={e.id}>
          <td>{e.name}</td>
          <td>
            <ButtonGroup size="sm">
              <Button color="danger" onClick={() => this.props.deleteEnvironment(e.id)}>
                <FontAwesomeIcon icon={faTimes} />
              </Button>
            </ButtonGroup>
          </td>
        </tr>
      ));
    }

    return (
      <div>
        <h1>Environments</h1>
        <hr />
        <div className="row">
          <div className="col-md-6">
            <NewEnvironmentForm addNewEnvironment={this.props.addNewEnvironment} />
            <br />
            <Table>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>&nbsp;</th>
                </tr>
              </thead>
              <tbody>
                {tableContent}
              </tbody>
            </Table>
          </div>
        </div>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  availableEnvironments: state.environments.available
});

const mapDispatchToProps = (dispatch) => ({
  getAllEnvironments: () => dispatch(getAllEnvironments()),
  addNewEnvironment: (name) => dispatch(addNewEnvironment(name)),
  deleteEnvironment: (id) => dispatch(deleteEnvironment(id))
});

export default connect(mapStateToProps, mapDispatchToProps)(EnvironmentsView);