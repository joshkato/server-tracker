import React from 'react';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import Navigation from './components/Navigation';
import IndexView from './views/IndexView';
import EnvironmentsView from './views/EnvironmentsView';
import ServersView from './views/ServersView';

class App extends React.Component {
  render() {
    return (
      <Router>
        <Navigation />
        <div className="container-fluid">
          <Route path="/" exact component={IndexView} />
          <Route path="/environments" component={EnvironmentsView} />
          <Route path="/servers" component={ServersView} />
        </div>
      </Router>
    );
  }
}

export default App;