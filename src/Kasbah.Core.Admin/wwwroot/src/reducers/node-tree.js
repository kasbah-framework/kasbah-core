import { createReducer } from 'utils';

import { RECEIVE_NODES, TOGGLE_NODE, CLEAR_CHILDREN } from 'actions/node-tree';

const initialState = [{alias: '[Root]', id: null, hasChildren: true, parent: null}];
export default createReducer(initialState, {
    [RECEIVE_NODES] : (state, payload) => {
        let ret = [...state];
        for (var i = 0; i < payload.data.length; i++) {
            payload.data[i].parent = payload.parent;
            ret.push(payload.data[i]);
        }
        return ret;
    },
    [TOGGLE_NODE] : (state, payload) => {
        let currentlyExpanded = payload.node.expanded;
        const index = state.indexOf(payload.node);
        const updated = Object.assign({}, state[index], {
            expanded: !currentlyExpanded
        });
        return [
                ...state.slice(0, index),
                updated,
                ...state.slice(index + 1)
            ];
    },
    [CLEAR_CHILDREN] : (state, payload) => {
        const toRemove = state.filter(ent => ent.parent === payload.node.id);
        return state.filter(ent => toRemove.indexOf(ent) == -1);
    }
});
