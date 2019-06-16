import React from 'react';
import { Link } from 'react-router-dom'
import { Button } from 'reactstrap';
import { toast } from 'react-toastify';

export default class IndexView extends React.Component {
  render() {
    return (
      <div>
        <h1>Server Tracker</h1>
        <hr />
        <p>Simple application to keep track of servers and which environments they reside in.</p>
        <p>
          Use the <Link to="/environments">Environments</Link> and <Link to="/servers">Servers</Link> navigation options on the navigation bar to access this application's features.
        </p>
      </div>
    );
  }
}
