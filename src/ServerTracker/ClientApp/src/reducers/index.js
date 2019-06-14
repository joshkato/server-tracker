import { combineReducers } from 'redux';
import environments from './environments';
import servers from './servers'

export default combineReducers({
  environments,
  servers
});
