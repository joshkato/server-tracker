import _ from 'lodash';
import { SERVERS_ADD, SERVERS_DEL, SERVERS_GET_ALL, SERVERS_RECV_FOR_ENV, SERVERS_RECV_ALL, SERVERS_UPDATE } from './../actions/servers'
import { createNewServer, getAllServers, removeServer, updateServer } from './../client';

const defaultState = {
  all: {}
};

const handlers = {};

handlers[SERVERS_ADD] = (state, action) => {
  createNewServer(action.server);
  return state;
}

handlers[SERVERS_DEL] = (state, action) => {
  removeServer(action.serverId);
  return state;
}

handlers[SERVERS_GET_ALL] = (state, action) => {
  getAllServers();
  return state;
}

handlers[SERVERS_RECV_FOR_ENV] = (state, action) => {
  return state;
}

handlers[SERVERS_RECV_ALL] = (state, action) => {
  return {
    ...state,
    all: _.groupBy(action.data, 'environmentId')
  };
}

handlers[SERVERS_UPDATE] = (state, action) => {
  updateServer(action.server);
  return state;
}

const handleAction = (state = defaultState, action) => {
  if (handlers[action.type]) {
    return handlers[action.type](state, action);
  }

  return state;
}

export default handleAction;