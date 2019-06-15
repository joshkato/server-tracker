const defaultState = {};

const handlers = {};

handlers['ERR_SERVER'] = (state, action) => {
  console.error(action.message);
  return state;
}

const handleAction = (state = defaultState, action) => {
  if (handlers[action.type]) {
    return handlers[action.type](state, action);
  }

  return state;
}

export default handleAction;