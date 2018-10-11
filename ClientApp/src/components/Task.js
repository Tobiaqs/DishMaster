import React, { Component } from 'react';

export class Task extends Component {
    render() {
        return <div>
            <h1>A task</h1>
            {this.props.match.params.taskId}
        </div>;
    }
}