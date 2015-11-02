import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import NodeList from 'components/NodeList';
import NodeVersionList from 'components/NodeVersionList';
import NodeVersionDisplay from 'components/NodeVersionDisplay';
import {
    fetchNodeVersions,
    fetchNodeVersion,
    fetchChildren,
    toggleNode,
    clearChildren } from 'actions/nodes';

const actionCreators = {
    toggleNode : (node) => toggleNode(node),
    fetchChildren: (node) => fetchChildren(node.id),
    clearChildren: (node) => clearChildren(node),
    fetchNodeVersions: (node) => fetchNodeVersions(node),
    fetchNodeVersion: (id, version) => fetchNodeVersion(id, version.id)
};

const mapStateToProps = (state) => ({
    nodeTree : state.nodeTree
});
const mapDispatchToProps = (dispatch) => ({
    actions : bindActionCreators(actionCreators, dispatch)
});
export class HomeView extends React.Component {
    static propTypes = {
        actions: React.PropTypes.object,
        nodeTree: React.PropTypes.object
    }

    constructor() {
        super();

        this.state = {};
    }

    componentWillMount() {
        this.props.actions.fetchChildren({ id: null });
    }

    handleNodeSelected(node) {
        this.props.actions.fetchNodeVersions(node.id);
        this.setState({ selectedNode: node });
    }

    handleVersionSelected(version) {
        this.props.actions.fetchNodeVersion(this.state.selectedNode.id, version);
        this.setState({ selectedVersion: version });
    }

    handleToggleNode(node) {
        this.props.actions.toggleNode(node);
        if (node.expanded) {
            this.props.actions.clearChildren(node);
        }
        else {
            this.props.actions.fetchChildren(node);
        }
    }

    render () {
        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-lg-2 col-md-3 col-node-list'>
                        <NodeList
                            parent={null}
                            nodeTree={this.props.nodeTree}
                            onNodeSelected={this.handleNodeSelected.bind(this)}
                            onToggleNode={this.handleToggleNode.bind(this)} />
                    </div>
                    <div className='col-lg-2 col-md-3 col-node-list'>
                        <NodeVersionList
                            selectedNode={this.state.selectedNode}
                            versions={this.props.nodeTree.versions}
                            onVersionSelected={this.handleVersionSelected.bind(this)} />
                    </div>
                    <div className='col-lg-8 col-md-6'>
                        <NodeVersionDisplay
                            selectedNode={this.state.selectedNode}
                            selectedVersion={this.state.selectedVersion}
                            items={this.props.nodeTree.items} />
                    </div>
                </div>
            </div>
        );
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(HomeView);
