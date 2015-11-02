import React from 'react';

class FieldEditor extends React.Component {
    static propTypes = {
        field: React.PropTypes.string.isRequired,
        value: React.PropTypes.any.isRequired,
        onChange: React.PropTypes.func.isRequired
    }

    render() {
        return (
            <div className='form-group'>
                <dt htmlFor={this.props.field}>
                    {this.props.field}
                </dt>
                <dd>
                    <input
                        id={this.props.field}
                        type='text'
                        value={this.props.value}
                        onChange={this.props.onChange.bind(this, this.props.field)} />
                </dd>
            </div>
        );
    }
}

export default class NodeVersionDisplay extends React.Component {
    _renderFields(obj) {
        var fields = Object.keys(obj);

        return (
            <div>
                <dl>
                    {fields.map(f =>
                        <FieldEditor
                            key={f}
                            field={f}
                            value={obj[f]}
                            onChange={this.props.onChange} />)}
                </dl>
                <pre>{JSON.stringify(obj)}</pre>
                <button className='btn btn-primary'>Save</button>
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
