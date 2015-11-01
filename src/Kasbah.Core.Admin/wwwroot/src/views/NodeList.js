import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import Node from './Node';
import { fetchChildren, clearChildren, toggleNode } from 'actions/node-tree';

const actionCreators = {
    toggleNode : (node) => toggleNode(node),
    fetchChildren: (node) => fetchChildren(node.id),
    clearChildren: (node) => clearChildren(node)
};

const mapStateToProps = (state) => ({
    nodeTree : state.nodeTree
});
const mapDispatchToProps = (dispatch) => ({
    actions : bindActionCreators(actionCreators, dispatch)
});
export class NodeList extends React.Component {
    static propTypes = {
        actions: React.PropTypes.object,
        nodeTree: React.PropTypes.object
    }

    componentWillMount() {
        this.props.actions.fetchChildren({ id: null });
    }

    toggleNode(node) {
        this.props.actions.toggleNode(node);
        if (node.expanded) {
            this.props.actions.clearChildren(node);
        }
        else {
            this.props.actions.fetchChildren(node);
        }
    }

    _renderChildren() {
        var children = [];
        for (var k in this.props.nodeTree) {
            var ent = this.props.nodeTree[k];
            if (ent.parent === null) {
                children.push(ent);
            }
        }

        return children.map(ent => (
            <Node
                key={ent.id}
                node={ent}
                nodeTree={this.props.nodeTree}
                onToggle={this.toggleNode.bind(this, ent)} />
        ));
    }

    render() {
        return (
            <ul className='node-list'>
                {this._renderChildren()}
            </ul>
        );
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NodeList);
