import React from 'react';
import NodeList from './NodeList';

class NodeExpandButton extends React.Component {
    render() {
        if (this.props.node.hasChildren) {
            const iconClass = 'fa fa-' + (this.props.expanded ? 'minus-square-o' : 'plus-square-o');

            return (
                <button className='toggle' onClick={this.props.onClick}>
                    <i className={iconClass}></i>
                </button>
            );
        }

        return null;
    }
}

export default class Node extends React.Component {
    render() {
        return (
            <li>
                <NodeExpandButton
                    node={this.props.node}
                    expanded={this.props.expanded}
                    onClick={this.props.onToggle} />

                <button onClick={this.props.onSelect}>
                    <i className='fa fa-files-o' />
                    {this.props.node.alias}
                </button>

                {this.props.expanded ? <NodeList parent={this.props.node.id} /> : null}
            </li>
        );
    }
}
