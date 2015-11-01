import React from 'react';
import { Router, Route, Link } from 'react-router'

export default class extends React.Component {
    render () {
        return (
            <nav className="navbar navbar-light bg-faded">
                <a className="navbar-brand" href="#">KASBAH</a>

                <ul className="nav navbar-nav">
                    <li className="nav-item">
                        <Link className="nav-link" to={`/`}>Content</Link>
                    </li>
                </ul>
            </nav>
        );
    }
}
