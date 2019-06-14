import { ENV_ADD, ENV_DEL, ENV_GET_ALL, ENV_RECV_ALL } from './../actions/environments';
import { createNewEnvironment, getAllEnvironments, removeEnvironment } from './../client';

const defaultState = {
  available: []
};

const handlers = [];

handlers[ENV_ADD] = (state, action) => {
  createNewEnvironment(action.envName);
  return state;
}

handlers[ENV_DEL] = (state, action) => {
  removeEnvironment(action.id);
  return state;
}

handlers[ENV_GET_ALL] = (state, action) => {
  getAllEnvironments();
  return state;
}

handlers[ENV_RECV_ALL] = (state, action) => {
  return {
    ...state,
    // I guess we'll just trust the server will send us back
    // all of the environments.
    available: action.data
  }
}

const handleAction = (state = defaultState, action) => {
  if (handlers[action.type]) {
    return handlers[action.type](state, action);
  }

  return state;
}

export default handleAction;