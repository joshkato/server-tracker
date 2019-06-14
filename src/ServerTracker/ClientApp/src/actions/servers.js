export const SERVERS_GET_ALL      = 'SERVERS_GET_ALL';
export const SERVERS_GET_FOR_ENV  = 'SERVERS_GET_FOR_ENV';
export const SERVERS_ADD          = 'SERVERS_ADD';
export const SERVERS_DEL          = 'SERVERS_DEL';
export const SERVERS_RECV_ALL     = 'SERVERS_RECV_ALL';
export const SERVERS_RECV_FOR_ENV = 'SERVERS_RECV_FOR_ENV';
export const SERVERS_UPDATE       = 'SERVERS_UPDATE';

export const addServer = (server) => {
  return {
    type: SERVERS_ADD,
    server
  };
}

export const deleteServer = (serverId) => {
  return {
    type: SERVERS_DEL,
    serverId
  };
}

export const getAllServers = () => {
  return {
    type: SERVERS_GET_ALL
  };
}

export const getAllServersInEnvironment = (envId) => {
  return {
    type: SERVERS_GET_FOR_ENV,
    envId
  };
}

export const updateServer = (server) => {
  return {
    type: SERVERS_UPDATE,
    server
  };
}