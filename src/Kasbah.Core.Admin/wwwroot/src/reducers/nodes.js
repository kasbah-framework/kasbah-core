import { createReducer } from 'utils';

import { RECEIVE_NODES, TOGGLE_NODE, CLEAR_CHILDREN, RECEIVE_NODE_VERSIONS } from 'actions/nodes';

function clone(obj) {
    return JSON.parse(JSON.stringify(obj));
}

const initialState = { nodes: {}, versions: {} };
export default createReducer(initialState, {
    [RECEIVE_NODES] : (state, payload) => {
        let ret = clone(state);

        for (var i = 0; i < payload.data.length; i++) {
            ret.nodes[payload.data[i].id] = payload.data[i];
        }

        return ret;
    },
    [TOGGLE_NODE] : (state, payload) => {
        let ret = clone(state);

        ret.nodes[payload.node.id].expanded = !ret.nodes[payload.node.id].expanded;

        return ret;
    },
    [CLEAR_CHILDREN] : (state, payload) => {
        let ret = clone(state);

        for (var k in ret.nodes) {
            if (ret.nodes[k].parent == payload.node.id && ret.nodes[k].id !== null) {
                delete ret[k];
            }
        }

        return ret;
    },
    [RECEIVE_NODE_VERSIONS]: (state, payload) => {
        let ret = clone(state);

        ret.versions[payload.node] = payload.data;

        return ret;
    }
});
