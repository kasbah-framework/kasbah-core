import React from 'react';
import moment from 'moment';

export default class NodeDetail extends React.Component {
    render() {
        if (this.props.node == null) {
            return null;
        }

        var friendlyType = function (fullType) {
            return fullType.split(',')[0].split('.').splice(-1, 1)[0];
        }

        return (
            <dl>
                <dt>Alias</dt>
                <dd>{this.props.node.alias}</dd>

                <dt>Type</dt>
                <dd>
                    <abbr title={this.props.node.type}>{friendlyType(this.props.node.type)}</abbr><br />
                    <small>{this.props.node.type}</small>
                </dd>

                <dt>Has children</dt>
                <dd>{this.props.node.hasChildren ? 'yes' : 'no'}</dd>

                <dt>Parent</dt>
                <dd>{this.props.node.parent || 'null'}</dd>

                <dt>Id</dt>
                <dd>{this.props.node.id}</dd>

                <dt>Raw</dt>
                <dd><pre>{JSON.stringify(this.props.node)}</pre></dd>
            </dl> );
    }
}
