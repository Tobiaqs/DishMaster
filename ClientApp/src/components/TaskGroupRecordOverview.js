import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { ListGroupItem, Glyphicon, ListGroup } from 'react-bootstrap';
import { ExpandableCollection } from './ExpandableCollection';

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
                    <ExpandableCollection>
                        {this.props.taskGroupRecords.map(taskGroupRecord => <ListGroupItem
                            onClick={() => this.showTaskGroupRecord(taskGroupRecord)}
                            key={taskGroupRecord.id}>
                            {new Date(taskGroupRecord.date).toLocaleString("nl-NL", { weekday: "long", day: "numeric", month: "long", year: "numeric" })}
                            </ListGroupItem>
                        )}
                    </ExpandableCollection>
                    {this.props.taskGroupRecords.length === 0 ?
                        <ListGroupItem disabled>
                            Er zijn nog geen taakverdelingen.
                        </ListGroupItem>
                    : null}
                    {this.props.tasks.length > 0 ?
                        <ListGroupItem onClick={this.createTaskGroupRecord}>
                            <Glyphicon glyph="plus" /> <i>Maak een voorlopige taakverdeling&#8230;</i>
                        </ListGroupItem>
                    : null}
                </ListGroup>
            </div>
            {this.state.selectedTaskGroupRecordId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/taskGroupRecord/' + this.state.selectedTaskGroupRecordId} push={true} /> : null}
            {this.state.createTaskGroupRecord ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/create-task-group-record'} push={true} /> : null}
        </div>;
    }
}