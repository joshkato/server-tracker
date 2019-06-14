export const ENV_ADD      = 'ENV_ADD';
export const ENV_DEL      = 'ENV_DEL';
export const ENV_GET_ALL  = 'ENV_GET_ALL';
export const ENV_RECV_ALL = 'ENV_RECV_ALL';

export const addNewEnvironment = name => {
  return {
    type: ENV_ADD,
    envName: name
  };
}

export const deleteEnvironment = id => {
  return {
    type: ENV_DEL,
    id
  };
}

export const getAllEnvironments = _ => {
  return {
    type: ENV_GET_ALL
  };
}
