import React from 'react';
import Node from './Node';

export default class NodeList extends React.Component {
    static propTypes = {
        nodeTree: React.PropTypes.object.isRequired,
        parent: React.PropTypes.oneOfType([
            React.PropTypes.object,
            React.PropTypes.string
        ]),
        onToggleNode: React.PropTypes.func.isRequired,
        onNodeSelected: React.PropTypes.func.isRequired
    }

    _renderChildren() {
        var children = Object.keys(this.props.nodeTree.nodes)
            .map(k => this.props.nodeTree.nodes[k])
            .filter(ent => ent.parent === this.props.parent);

        return children.map(ent => (
            <Node
                key={ent.id}
                node={ent}
                nodeTree={this.props.nodeTree}
                onToggle={this.props.onToggleNode}
                onSelect={this.props.onNodeSelected}
                onCreate={this.props.onCreateNode} />
        ));
    }

    _renderCreateButton() {
        return (
            <li>
                <button onClick={this.props.onCreateNode.bind(this, this.props.parent)}>
                    <i className='fa fa-file-o' />
                    <span>Add node here</span>
                </button>
            </li>);
    }

    render() {
        return (
            <ul className='node-list'>
                {this._renderChildren()}
                {this._renderCreateButton()}
            </ul>
        );
    }
}
