import React from 'react';
import moment from 'moment';

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
                        <li key={ent.id}>
                            <button onClick={this.props.onVersionSelected.bind(this, ent)}>
                                <i className='fa fa-code-fork' /> {moment.utc(ent.modified).format()}
                            </button>
                        </li>
                    )}
                </ul>
            </div>
        );
    }
}
