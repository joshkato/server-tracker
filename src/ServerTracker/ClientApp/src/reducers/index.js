import { combineReducers } from 'redux';
import environments from './environments';
import servers from './servers'
import serverErrors from './serverErrors';

export default combineReducers({
  environments,
  servers,
  serverErrors
});
