import { HubConnectionBuilder } from '@aspnet/signalr';
import store from './store';

const SERVER_URL = 'http://localhost:5000/ws/server-tracker';
const connection = new HubConnectionBuilder()
  .withUrl(SERVER_URL)
  .build();

const dispatchError = (error) => {
  if (!error) {
    return;
  }

  console.error(error);

  const errorMessage = {
    type: 'CLIENT_SERVER_ERROR',
    error: {
      title: 'Server Connection Error',
      message: 'Connection to the server failed or was interrupted.'
    }
  };

  store.dispatch(errorMessage);
};

const dispatchMessage = (message) => {
  if (!message.type) {
    console.error(`Message recieved from server is missing 'type'!\n${JSON.stringify(message)}`);
    return;
  }

  store.dispatch(message);
}

const invoke = (functionName, ...args) => {
  connection.invoke(functionName, ...args).catch(dispatchError);
}

export const connect = () => {
  connection.on('DispatchMessage', dispatchMessage);
  return connection.start()
    .catch(dispatchError);
};

export const createNewEnvironment = (name) => {
  invoke('CreateNewEnvironment', name);
}

export const createNewServer = (server) => {
  invoke('CreateNewServer', server);
}

export const getAllEnvironments = () => {
  invoke('GetAllEnvironments');
}

export const getAllServers = () => {
  invoke('GetAllServers');
}

export const getAllServersForEnvironment = (envId) => {
  invoke('GetServersForEnvironment', envId);
}

export const removeEnvironment = (id) => {
  invoke('RemoveEnvironment', id);
}

export const removeServer = (id) => {
  invoke('RemoveServer', id);
};

export const updateServer = (server) => {
  invoke('UpdateServer', server);
}