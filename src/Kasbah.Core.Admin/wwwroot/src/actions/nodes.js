import fetch from 'isomorphic-fetch';

export const REQUEST_NODES = 'REQUEST_NODES';
function requestNodes(parent) {
    return {
        type: REQUEST_NODES,
        payload: {
            parent
        }
    }
}

export const RECEIVE_NODES = 'RECEIVE_NODES';
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


export const REQUEST_NODE_VERSIONS = 'REQUEST_NODE_VERSIONS';
function requestNodeVersions(node) {
    return {
        type: REQUEST_NODE_VERSIONS,
        payload: {
            node
        }
    }
}

export const RECEIVE_NODE_VERSIONS = 'RECEIVE_NODE_VERSIONS';
function receiveNodeVersions(node, data) {
    return {
        type: RECEIVE_NODE_VERSIONS,
        payload: {
            node,
            data
        }
    }
}

export function fetchNodeVersions(node) {
    return dispatch => {
        dispatch(requestNodeVersions(node))
            return fetch(`http://localhost:5004/api/versions/${node}`)
                .then(response => response.json())
                .then(json => dispatch(receiveNodeVersions(node, json)))
    }
}


export const REQUEST_NODE_VERSION = 'REQUEST_NODE_VERSION';
function requestNodeVersion(id, version) {
    return {
        type: REQUEST_NODE_VERSION,
        payload: {
            id,
            version
        }
    }
}

export const RECEIVE_NODE_VERSION = 'RECEIVE_NODE_VERSION';
function receiveNodeVersion(id, version, data) {
    return {
        type: RECEIVE_NODE_VERSION,
        payload: {
            id,
            version,
            data
        }
    }
}

export function fetchNodeVersion(id, version) {
    return dispatch => {
        dispatch(requestNodeVersions(id, version))
            return fetch(`http://localhost:5004/api/version/${id}/${version}`)
                .then(response => response.json())
                .then(json => dispatch(receiveNodeVersion(id, version, json)))
    }
}

function notifyUpdateItem(node, version, field, value) {
    return {
        type: UPDATE_ITEM,
        payload: {
            node,
            version,
            field,
            value
        }
    }
}

export const UPDATE_ITEM = 'UPDATE_ITEM';
export function updateItem(node, version, field, value) {
    return dispatch => {
        dispatch(notifyUpdateItem(node, version, field, value));
    }
}
