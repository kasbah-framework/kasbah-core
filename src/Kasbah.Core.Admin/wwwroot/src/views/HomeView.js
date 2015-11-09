import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import NodeList from 'components/NodeList';
import NodeDetail from 'components/NodeDetail';
import NodeVersionList from 'components/NodeVersionList';
import NodeVersionDisplay from 'components/NodeVersionDisplay';
import {
    fetchNodeVersions,
    fetchNodeVersion,
    fetchChildren,
    toggleNode,
    clearChildren,
    updateItem,
    createNode,
    createNodeVersion,
    addField,
    save,
    setActiveVersion } from 'actions/nodes';

const actionCreators = {
    toggleNode: (node) => toggleNode(node),
    fetchChildren: (node) => fetchChildren(node.id),
    clearChildren: (node) => clearChildren(node),
    fetchNodeVersions: (node) => fetchNodeVersions(node),
    fetchNodeVersion: (id, version) => fetchNodeVersion(id, version.id),
    updateItem: (node, version, field, value) => updateItem(node, version, field, value),
    createNode: (parent, alias, type) => createNode(parent, alias, type),
    createNodeVersion: (node) => createNodeVersion(node),
    addField: (node, version, name) => addField(node, version, name),
    save: (node, version, values) => save(node, version, values),
    setActiveVersion: (node, version) => setActiveVersion(node, version)
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

    handleFieldChange(field, event) {
        if (!this.state.selectedNode || !this.state.selectedVersion) { return; }
        this.props.actions.updateItem(this.state.selectedNode.id, this.state.selectedVersion.id, field, event.target.value);
    }

    handleCreateNode(parent) {
        const alias = prompt('alias'),
            type = prompt('type');

        if (alias && type) {
            this.props.actions.createNode(parent, alias, type);
        }
    }

    handleCreateNodeVersion(node) {
        this.props.actions.createNodeVersion(node.id);
    }

    handlAddField() {
        const name = prompt('name');

        if (name) {
            this.props.actions.addField(this.state.selectedNode.id, this.state.selectedVersion.id, name);
        }
    }

    handleSetActive() {
        this.props.actions.setActiveVersion(this.state.selectedNode.id, this.state.selectedVersion.id);
    }

    handleSave() {
        const values = this.props.nodeTree.items[this.state.selectedNode.id][this.state.selectedVersion.id];
        this.props.actions.save(this.state.selectedNode.id, this.state.selectedVersion.id, values)
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
                            onToggleNode={this.handleToggleNode.bind(this)}
                            onCreateNode={this.handleCreateNode.bind(this)} />
                    </div>
                    <div className='col-lg-2 col-md-3 col-node-list'>
                        <NodeVersionList
                            node={this.state.selectedNode}
                            versions={this.props.nodeTree.versions}
                            onVersionSelected={this.handleVersionSelected.bind(this)}
                            onCreateNodeVersion={this.handleCreateNodeVersion.bind(this)} />
                        <hr />
                        <NodeDetail
                            node={this.state.selectedNode} />
                    </div>
                    <div className='col-lg-8 col-md-6'>
                        <NodeVersionDisplay
                            selectedNode={this.state.selectedNode}
                            selectedVersion={this.state.selectedVersion}
                            items={this.props.nodeTree.items}
                            onChange={this.handleFieldChange.bind(this)}
                            onAddField={this.handlAddField.bind(this)}
                            onSave={this.handleSave.bind(this)}
                            onSetActive={this.handleSetActive.bind(this)} />
                    </div>
                </div>
            </div>
        );
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(HomeView);
