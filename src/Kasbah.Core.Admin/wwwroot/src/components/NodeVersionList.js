import React from 'react';

export default class NodeVersionList extends React.Component {
    render() {
        if (this.props.selectedNode == null) {
            return null;
        }

        const versions = this.props.versions[this.props.selectedNode.id];
        if (versions == null || versions.length == 0) {
            return null;
        }

        return (
            <div className='node-version-list'>
                <p>Node versions for <strong>{this.props.selectedNode.alias}</strong></p>
                <ul className='node-list'>
                    {versions.map(ent =>
                        <li key={ent.id}><button onClick={this.props.onVersionSelected.bind(this, ent)}>{ent.id}</button></li>
                    )}
                </ul>
            </div>
        );
    }
}
