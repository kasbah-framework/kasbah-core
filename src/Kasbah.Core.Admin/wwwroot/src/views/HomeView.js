import React from 'react';
import NodeList from './NodeList';

export default class HomeView extends React.Component {
    render () {
        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-lg-2 col-md-3 col-node-list'>
                        <NodeList parent={null} />
                    </div>
                    <div className='col-lg-10 col-md-9'>
                        <p>Content.</p>
                    </div>
                </div>
            </div>
        );
    }
}
