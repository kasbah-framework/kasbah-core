import React from 'react';

export default class Node extends React.Component {

    _renderChildren() {
        if (!this.props.node.expanded || !this.props.node.hasChildren) {
            return null;
        }

        var children = [];
        for (var k in this.props.nodeTree.nodes) {
            var ent = this.props.nodeTree.nodes[k];
            if (ent.parent === this.props.node.id) {
                children.push(ent);
            }
        }

        return (
            <ul className='node-list'>
            {children.map(ent => (
                <Node
                    key={ent.id}
                    node={ent}
                    nodeTree={this.props.nodeTree}
                    onToggle={this.props.onToggle}
                    onSelect={this.props.onSelect.bind(this, ent)} />
            ))}
            </ul>
        );
    }

    render() {
        const iconClass = 'fa fa-' + (this.props.node.expanded ? 'minus-square-o' : 'plus-square-o');

        const toggleButton = this.props.node.hasChildren ? <button className='toggle' onClick={this.props.onToggle}><i className={iconClass}></i></button> : null;

        return (
            <li>
                {toggleButton}

                <button onClick={this.props.onSelect.bind(this, this.props.node)}>
                    <i className='fa fa-files-o' />
                    {this.props.node.alias}
                </button>

                {this._renderChildren()}
            </li>
        );
    }
}
