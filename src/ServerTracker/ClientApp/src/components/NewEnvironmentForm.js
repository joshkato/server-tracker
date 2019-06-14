import React from 'react';
import { Button, Input, InputGroup, InputGroupAddon } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

export default class NewEnvironmentForm extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      envName: ''
    }

    this.x = React.createRef();

    this.handleAddClick = this.handleAddClick.bind(this);
    this.handleKeyUp = this.handleKeyUp.bind(this);
    this.handleNewEnvironmentNameChanged = this.handleNewEnvironmentNameChanged.bind(this);
  }

  handleAddClick() {
    if (!this.props.addNewEnvironment) {
      return;
    }

    this.props.addNewEnvironment(this.state.envName);
    this.setState({
      envName: ''
    });
  }

  handleKeyUp(event) {
    if (event.keyCode === 13) {
      this.handleAddClick();
    }
  }

  handleNewEnvironmentNameChanged(event) {
    this.setState({
      envName: event.target.value
    })
  }

  render() {
    return (
      <div>
        <InputGroup>
          <InputGroupAddon addonType="prepend">
            <Button onClick={this.handleAddClick}><FontAwesomeIcon icon={faPlus} /></Button>
            <Input type="text" placeholder="Environment Name" value={this.state.envName}
                   onChange={this.handleNewEnvironmentNameChanged}
                   onKeyUp={this.handleKeyUp} ref={this.x} />
          </InputGroupAddon>
        </InputGroup>
      </div>
    );
  }
}