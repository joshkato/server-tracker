import React from 'react';
import { connect } from 'react-redux';
import { Button, Col, Form, FormGroup, Input, Label, Table } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faPlus, faTimes } from '@fortawesome/free-solid-svg-icons';
import _ from 'lodash';
import { getAllEnvironments } from './../actions/environments';
import { addServer, deleteServer, getAllServers, updateServer } from './../actions/servers';

function getDefaultServer() {
  return {
    serverId: 0,
    serverName: '',
    serverDomainName: '',
    serverOs: 'linux-x86',
    serverIpAddress: '127.0.0.1'
  }
}

class ServersView extends React.Component {
  constructor() {
    super();

    const defaultServer = getDefaultServer();
    this.state = {
      selectedEnvironmentId: 1,
      ...defaultServer
    };

    this.handleAddServerClick = this.handleAddServerClick.bind(this);
    this.handleUpdateServerClick = this.handleUpdateServerClick.bind(this);
    this.handleEnvironmentChanged = this.handleEnvironmentChanged.bind(this);
    this.handleServerNameChanged = this.handleServerNameChanged.bind(this);
    this.handleDomainNameChanged = this.handleDomainNameChanged.bind(this);
    this.handleIpAddressChanged = this.handleIpAddressChanged.bind(this);
    this.handleOsChanged = this.handleOsChanged.bind(this);
    this.renderServerRows = this.renderServerRows.bind(this);
    this.renderServerFormButton = this.renderServerFormButton.bind(this);
    this.setLoadedServer = this.setLoadedServer.bind(this);
  }

  handleAddServerClick(event) {
    this.props.addServer({
      name: this.state.serverName,
      environmentId: parseInt(this.state.selectedEnvironmentId),
      domainName: this.state.serverDomainName,
      ipAddress: this.state.serverIpAddress,
      operatingSystem: this.state.serverOs
    });

    const defaultServer = getDefaultServer();
    this.setState({
      ...defaultServer
    });
  }

  handleUpdateServerClick(event) {
    this.props.updateServer({
      id: this.state.serverId,
      name: this.state.serverName,
      environmentId: parseInt(this.state.selectedEnvironmentId),
      domainName: this.state.serverDomainName,
      ipAddress: this.state.serverIpAddress,
      operatingSystem: this.state.serverOs
    });

    const defaultServer = getDefaultServer();
    this.setState({
      ...defaultServer
    });
  }

  handleEnvironmentChanged(event) {
    this.setState({
      selectedEnvironmentId: parseInt(event.target.value)
    });
  }

  handleServerNameChanged(event) {
    this.setState({
      serverName: event.target.value
    });
  }

  handleDomainNameChanged(event) {
    this.setState({
      serverDomainName: event.target.value
    });
  }

  handleIpAddressChanged(event) {
    this.setState({
      serverIpAddress: event.target.value
    });
  }

  handleOsChanged(event) {
    this.setState({
      serverOs: event.target.value
    });
  }

  componentDidMount() {
    this.props.getAllEnvironments();
    this.props.getAllServers();
  }

  setLoadedServer(server) {
    this.setState({
      serverId: server.id,
      serverName: server.name,
      serverDomainName: server.domainName,
      serverOs: server.operatingSystem,
      serverIpAddress: server.ipAddress
    });
  }

  renderServerFormButton() {
    if (this.state.serverId) {
      return (
        <Button color="warning" onClick={this.handleUpdateServerClick}>
          <FontAwesomeIcon icon={faEdit} />&nbsp;Update
        </Button>
      )
    }

    return (
      <Button color="success" onClick={this.handleAddServerClick}>
        <FontAwesomeIcon icon={faPlus} />&nbsp;Add
      </Button>
    );
  }

  renderServerRows(envId) {
    const selectedGroup = this.props.availableServers[envId];
    if (!selectedGroup) {
      return (
        <tr>
          <td colSpan="5">
            <span>No servers exist in the selected environment.</span>
          </td>
        </tr>
      );
    }

    return _.map(selectedGroup, server => (
      <tr key={server.id}>
        <td>
          <Button onClick={() => this.setLoadedServer(server)}>
            <FontAwesomeIcon icon={faEdit} />
          </Button>
          <Button color="danger" onClick={() => this.props.deleteServer(server.id)}>
            <FontAwesomeIcon icon={faTimes} />
          </Button>
        </td>
        <td>{server.name}</td>
        <td>
          <code>{server.domainName}</code>
        </td>
        <td>
          <code>{server.ipAddress}</code>
        </td>
        <td>{server.operatingSystem}</td>
      </tr>
    ));
  }

  render() {
    return (
      <div>
        <h1>Servers</h1>
        <hr />
        <div className="row">
          <Col sm={12}>
            <div style={{ marginLeft: '25px', marginRight: '25px' }}>
              <FormGroup row>
                <Label>Environment</Label>
                <Input type="select" value={this.state.selectedEnvironmentId}
                       onChange={this.handleEnvironmentChanged}>
                  {_.map(this.props.availableEnvironments, (env) => (
                    <option key={env.id} value={env.id}>{env.name}</option>
                  ))}
                </Input>
              </FormGroup>
            </div>
          </Col>
        </div>
        <hr />
        <div className="row">
          <Col md={4}>
            <Form>
              <FormGroup row>
                <Label for="server_name" sm={3}>Server Name</Label>
                <Col sm={9}>
                  <Input type="text" id="server_name" placeholder="AppHost1"
                         value={this.state.serverName} onChange={this.handleServerNameChanged} />
                </Col>
              </FormGroup>
              <FormGroup row>
                <Label sm={3}>Domain Name</Label>
                <Col sm={9}>
                  <Input type="text" id="domain_name" placeholder="apphost1.domain.tld"
                         value={this.state.serverDomainName} onChange={this.handleDomainNameChanged} />
                </Col>
              </FormGroup>
              <FormGroup row>
                <Label sm={3}>IP Address</Label>
                <Col sm={9}>
                  <Input type="text" id="ip_address" placeholder="127.0.0.1"
                         value={this.state.serverIpAddress} onChange={this.handleIpAddressChanged} />
                </Col>
              </FormGroup>
              <FormGroup row>
                <Label sm={3}>OS</Label>
                <Col sm={9}>
                  <Input type="select" id="os" value={this.state.serverOs} onChange={this.handleOsChanged}>
                    <option value="linux-x86">Linux (32-Bit)</option>
                    <option value="linux-x64">Linux (64-Bit)</option>
                    <option value="osx">macOS</option>
                    <option value="windows-x86">Windows (32-Bit)</option>
                    <option value="windows-x64">Windows (64-Bit)</option>
                  </Input>
                </Col>
              </FormGroup>
            </Form>
            {this.renderServerFormButton()}
          </Col>
          <Col md={8}>
            <Table>
              <thead>
                <tr>
                  <th>&nbsp;</th>
                  <th>Server Name</th>
                  <th>Domain Name</th>
                  <th>IP Address</th>
                  <th>OS</th>
                </tr>
              </thead>
              <tbody>
                {this.renderServerRows(this.state.selectedEnvironmentId)}
              </tbody>
            </Table>
          </Col>
        </div>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  availableEnvironments: state.environments.available,
  availableServers: state.servers.all
});

const mapDispatchToProps = (dispatch) => ({
  addServer: (server) => dispatch(addServer(server)),
  deleteServer: (serverId) => dispatch(deleteServer(serverId)),
  getAllEnvironments: () => dispatch(getAllEnvironments()),
  getAllServers: () => dispatch(getAllServers()),
  updateServer: (server) => dispatch(updateServer(server))
});

export default connect(mapStateToProps, mapDispatchToProps)(ServersView);