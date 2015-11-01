import React from 'react';
import 'styles/core.scss';
import Navigation from 'components/Navigation';

export default class CoreLayout extends React.Component {
    static propTypes = {
        children : React.PropTypes.element
    }

    render () {
        return (
            <div className='page-container'>
                <Navigation />
                <div className='view-container'>
                    {this.props.children}
                </div>
            </div>
        );
    }
}
