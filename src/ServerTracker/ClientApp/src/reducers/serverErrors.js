import { toast } from 'react-toastify';

const defaultState = {};

const handlers = {};

handlers['ERR_SERVER'] = (state, action) => {
  toast.error(action.message, {
    autoClose: 7500,
    pauseOnHover: true,
    position: toast.POSITION.TOP_LEFT
  });

  return state;
}

const handleAction = (state = defaultState, action) => {
  if (handlers[action.type]) {
    return handlers[action.type](state, action);
  }

  return state;
}

export default handleAction;