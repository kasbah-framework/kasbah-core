import { combineReducers } from 'redux';
import { routerStateReducer } from 'redux-router';
import NodeTree from './node-tree';

export default combineReducers({
    nodeTree: NodeTree,
    router: routerStateReducer
});
