import React from 'react';

export default class NodeVersionDisplay extends React.Component {
    render() {
        if (this.props.selectedNode == null || this.props.selectedVersion == null) { return null; }

        const nodeVals = this.props.items[this.props.selectedNode.id];
        if (!nodeVals) { return null;}

        const val = nodeVals[this.props.selectedVersion.id];
        if (!val) { return null; }

        return <pre>{JSON.stringify(val)}</pre>;
    }
}
