import React from 'react';
import moment from 'moment';

class FieldEditor extends React.Component {
    static propTypes = {
        field: React.PropTypes.string.isRequired,
        value: React.PropTypes.any.isRequired,
        onChange: React.PropTypes.func.isRequired
    }

    render() {
        let editor = null;
        if (this.props.field == 'body')
        {
            editor = (
                <textarea
                    className='form-control'
                    id={this.props.field}
                    value={this.props.value}
                    onChange={this.props.onChange.bind(this, this.props.field)} /> );
        }
        else {
            editor = (
                <input
                    className='form-control'
                    id={this.props.field}
                    type='text'
                    value={this.props.value}
                    onChange={this.props.onChange.bind(this, this.props.field)} />);
        }
        return (
            <div className='form-group'>
                <label htmlFor={this.props.field}>
                    {this.props.field}
                </label>
                {editor}
            </div>
        );
    }
}

export default class NodeVersionDisplay extends React.Component {
    _renderFields(obj) {
        var fields = Object.keys(obj);

        return (
            <div>
                <p>Node version <strong>{this.props.selectedVersion.id}</strong> {moment.utc(this.props.selectedVersion.modified).format()}</p>
                <div>
                    {fields.map(f =>
                        <FieldEditor
                            key={f}
                            field={f}
                            value={obj[f]}
                            onChange={this.props.onChange} />)}
                    <button className='btn btn-sm' onClick={this.props.onAddField}>Add field</button>
                </div>
                <pre>{JSON.stringify(obj)}</pre>
                <button className='btn btn-primary' onClick={this.props.onSave}>Save</button>
                <button className='btn btn-success' onClick={this.props.onSetActive}>Set active</button>
            </div>
        );
    }

    render() {
        if (this.props.selectedNode == null || this.props.selectedVersion == null) {
            return null;
        }

        const values = this.props.items[this.props.selectedNode.id];
        if (!values) { return null;}

        const value = values[this.props.selectedVersion.id];
        if (!value) { return null; }

        return this._renderFields(value);
    }
}
