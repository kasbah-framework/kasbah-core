import { createReducer } from 'utils';

import { RECEIVE_NODES, TOGGLE_NODE, CLEAR_CHILDREN } from 'actions/node-tree';

function clone(obj) {
    return JSON.parse(JSON.stringify(obj));
}

const initialState = {};
export default createReducer(initialState, {
    [RECEIVE_NODES] : (state, payload) => {
        let ret = clone(state);

        for (var i = 0; i < payload.data.length; i++) {
            ret[payload.data[i].id] = payload.data[i];
        }

        return ret;
    },
    [TOGGLE_NODE] : (state, payload) => {
        let ret = clone(state);

        ret[payload.node.id].expanded = !ret[payload.node.id].expanded;

        return ret;
    },
    [CLEAR_CHILDREN] : (state, payload) => {
        let ret = clone(state);

        for (var k in ret) {
            if (ret[k].parent == payload.node.id && ret[k].id !== null) {
                delete ret[k];
            }
        }

        return ret;
    }
});
