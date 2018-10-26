import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { ListGroup, ListGroupItem } from 'react-bootstrap';

export class TaskGroupRecordOverview extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedTaskGroupRecordId: null,
            createTaskGroupRecord: false
        };

        this.showTaskGroupRecord = this.showTaskGroupRecord.bind(this);
        this.createTaskGroupRecord = this.createTaskGroupRecord.bind(this);
    }

    showTaskGroupRecord(taskGroupRecord) {
        this.setState({ selectedTaskGroupRecordId: taskGroupRecord.id });
    }

    createTaskGroupRecord() {
        this.setState({ createTaskGroupRecord: true });
    }

    render() {
        return <div>
            <div>
                <h4>Taakverdelingen</h4>
                <ListGroup>
                    {this.props.taskGroupRecords.map(taskGroupRecord => <ListGroupItem
                        onClick={() => this.showTaskGroupRecord(taskGroupRecord)}
                        key={taskGroupRecord.id}>
                        {new Date(taskGroupRecord.date).toLocaleString("nl-NL", { weekday: "long", day: "numeric", month: "long", year: "numeric" })}
                        </ListGroupItem>
                    )}
                    {this.props.taskGroupRecords.length === 0 ?
                        <ListGroupItem disabled key="none">
                            Er zijn nog geen taakverdelingen.
                        </ListGroupItem>
                    : null}
                    <ListGroupItem onClick={this.createTaskGroupRecord}><i>Maak een voorlopige taakverdeling&#8230;</i></ListGroupItem>
                </ListGroup>
            </div>
            {this.state.selectedTaskGroupRecordId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/taskGroupRecord/' + this.state.selectedTaskGroupRecordId} push={true} /> : null}
            {this.state.createTaskGroupRecord ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/create-task-group-record'} push={true} /> : null}
        </div>;
    }
}