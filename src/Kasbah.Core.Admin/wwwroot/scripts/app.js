import React from 'react';
import ReactDOM from 'react-dom';
import ContentNavigator from './components/ContentNavigator';
import Navigation from './components/Navigation';
import Router, { Route, HashLocation, RouteHandler } from 'react-router';

class App extends React.Component {
    render() {
        return (
            <div className="container">
                Hello world.
            </div>);
    }
}

class AppContainer extends React.Component {
    render() {
        return (
            <div>
                <Navigation />

                <Router>
                    <Route path="/" component={App} />
                    <Route path="/content" component={ContentNavigator} />
                </Router>
            </div>
        );
    }
}

ReactDOM.render(<AppContainer />, document.body);
