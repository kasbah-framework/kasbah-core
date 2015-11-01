import fetch from 'isomorphic-fetch';

export const REQUEST_NODES = 'REQUEST_NODES'
function requestNodes(parent) {
    return {
        type: REQUEST_NODES,
        payload: {
            parent
        }
    }
}

export const RECEIVE_NODES = 'RECEIVE_NODES'
function receiveNodes(parent, data) {
    return {
        type: RECEIVE_NODES,
        payload: {
            parent,
            data
        }
    }
}

export function fetchChildren(parent) {
    return dispatch => {
        dispatch(requestNodes(parent))
            return fetch(`http://localhost:5004/api/children?id=${parent}`)
                .then(response => response.json())
                .then(json => dispatch(receiveNodes(parent, json)))
    }
}

export const TOGGLE_NODE = 'TOGGLE_NODE';
export function toggleNode(node) {
    return dispatch => {
        dispatch({
            type : TOGGLE_NODE,
            payload: {
                node
            }
        })
    };
}

export const CLEAR_CHILDREN = 'CLEAR_CHILDREN';
export function clearChildren(node) {
    return dispatch => {
        dispatch({
            type : CLEAR_CHILDREN,
            payload: {
                node
            }
        })
    };
}
