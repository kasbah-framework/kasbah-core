import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import NodeList from 'components/NodeList';
import NodeVersionList from 'components/NodeVersionList';
import NodeVersionDisplay from 'components/NodeVersionDisplay';
import { fetchNodeVersions, fetchNodeVersion } from 'actions/nodes';

const actionCreators = {
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

    handleNodeSelected(node) {
        this.props.actions.fetchNodeVersions(node.id);
        this.setState({ selectedNode: node });
    }

    handleVersionSelected(version) {
        this.props.actions.fetchNodeVersion(this.state.selectedNode.id, version);
        this.setState({ selectedVersion: version });
    }

    render () {
        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-lg-2 col-md-3 col-node-list'>
                        <NodeList
                            parent={null}
                            onNodeSelected={this.handleNodeSelected.bind(this)} />
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
