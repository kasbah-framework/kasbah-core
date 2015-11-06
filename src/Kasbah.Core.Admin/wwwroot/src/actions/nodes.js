import fetch from 'isomorphic-fetch';

const API_BASE = 'http://localhost:5004';

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
            return fetch(`${API_BASE}/api/children?id=${parent}`)
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
            return fetch(`${API_BASE}/api/versions/${node}`)
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
            return fetch(`${API_BASE}/api/version/${id}/${version}`)
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

export const NODE_CREATED = 'NODE_CREATED';
function notifyNodeCreated(id) {
    return {
        type: NODE_CREATED,
        payload: {
            id
        }
    }
}

export function createNode(parent, alias, type) {
    const options = {
        method: 'POST',
        body: JSON.stringify({ parent, alias, type }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    };
    return dispatch => {
            return fetch(`${API_BASE}/api/node`, options)
                .then(response => response.json())
                .then(json => {
                    dispatch(notifyNodeCreated(json));
                    dispatch(fetchChildren(parent));
                })
    }
}


export const NODE_VERSION_CREATED = 'NODE_VERSION_CREATED';
function notifyNodeVersionCreated(node, id) {
    return {
        type: NODE_VERSION_CREATED,
        payload: {
            id
        }
    }
}

export function createNodeVersion(node) {
    const options = {
        method: 'POST',
        body: JSON.stringify({}),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    };
    return dispatch => {
            return fetch(`${API_BASE}/api/node/${node}/version`, options)
                .then(response => response.json())
                .then(json => {
                    dispatch(notifyNodeVersionCreated(json));
                    dispatch(fetchNodeVersions(node));
                })
    }
}
