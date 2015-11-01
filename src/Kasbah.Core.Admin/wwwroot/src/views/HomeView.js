import React from 'react';

export default class HomeView extends React.Component {
    render () {
        return (
            <div className='container-fluid'>
                <h1>Kasbah Core Administration</h1>
                <div className='row'>
                    <div className='col-lg-2 col-md-3'>
                        <p>Tree.</p>
                    </div>
                    <div className='col-lg-10 col-md-9'>
                        <p>Content.</p>
                    </div>
                </div>
            </div>
        );
    }
}
