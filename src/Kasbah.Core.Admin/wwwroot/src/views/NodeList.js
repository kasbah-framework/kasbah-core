import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import Node from './Node';
import { fetchChildren, clearChildren, toggleNode } from '../actions/node-tree';

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
        nodeTree: React.PropTypes.array
    }

    componentDidMount() {
    }

    toggleNode(node) {
        if (node.expanded) {
            this.props.actions.clearChildren(node);
        }
        else {
            // this.props.actions.clearChildren(node);
            this.props.actions.fetchChildren(node);
        }
        // this.props.actions.toggleNode(node);
    }

    _renderChildren() {
        var children = this.props.nodeTree.filter(ent => ent.parent === this.props.parent);
        return children.map(ent => (
            <Node
                key={Math.random()}
                node={ent}
                expanded={ent.expanded}
                onToggle={this.toggleNode.bind(this, ent)} />
        ));
    }

    render() {
        if (this.props.loading) {
            return <ul className='node-list'><li className='loading'>loading...</li></ul>;
        }

        return (
            <ul className='node-list'>
                {this._renderChildren()}
            </ul>
        );
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NodeList);
