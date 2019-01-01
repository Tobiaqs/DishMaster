import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { ListGroupItem, Glyphicon, ListGroup } from 'react-bootstrap';
import { Api } from '../Api';

export class TaskGroupRecordOverview extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedTaskGroupRecordId: null,
            createTaskGroupRecord: false,
            taskGroupRecords: [],
            loading: true,
            hasMore: false,
            offset: 0
        };
    }

    componentDidMount() {
        this.fetch();
    }

    fetch = () => {
        Api.getInstance().TaskGroup.ListTaskGroupRecords({
            taskGroupId: this.props.match.params.taskGroupId,
            offset: 0,
            count: 6,
            superficial: true
        }).then(result => {
            this.setState({
                taskGroupRecords: result.payload.slice(0, 5),
                offset: 5,
                hasMore: result.payload.length === 6,
                loading: false
            });
        });
    };

    showTaskGroupRecord = taskGroupRecord => {
        this.setState({ selectedTaskGroupRecordId: taskGroupRecord.id });
    };

    createTaskGroupRecord = () => {
        this.setState({ createTaskGroupRecord: true });
    };

    showMoreItems = () => {
        this.setState({ loading: true });

        Api.getInstance().TaskGroup.ListTaskGroupRecords({
            taskGroupId: this.props.match.params.taskGroupId,
            offset: this.state.offset,
            count: 11,
            superficial: true
        }).then(result => {
            this.setState({
                taskGroupRecords: this.state.taskGroupRecords.concat(result.payload.slice(0, 10)),
                offset: this.state.offset + 10,
                hasMore: result.payload.length === 11,
                loading: false
            });
        });
    };

    render() {
        return <div>
            <div>
                <h4>Taakverdelingen</h4>
                <i>Let op: gekleurde taakverdelingen zijn nog niet definitief gemaakt!</i>
                <ListGroup>
                    {this.state.taskGroupRecords.map(taskGroupRecord => <ListGroupItem
                            onClick={() => this.showTaskGroupRecord(taskGroupRecord)}
                            key={taskGroupRecord.id}
                            active={!taskGroupRecord.finalized}>
                            {new Date(taskGroupRecord.date).toLocaleString("nl-NL", { weekday: "long", day: "numeric", month: "long", year: "numeric" })}
                        </ListGroupItem>
                    )}
                    {!this.state.loading && this.state.hasMore ?
                        <ListGroupItem onClick={this.showMoreItems} key="show-more">
                            <Glyphicon glyph="chevron-down" /> <i>Laat meer zien&#8230;</i>
                        </ListGroupItem>
                    : null}
                    {this.state.loading ?
                        <ListGroupItem disabled={true} key="loading">
                            <Glyphicon glyph="glyphicon-option-horizontal" /> <i>Laden&#8230;</i>
                        </ListGroupItem>
                    : null}
                    {!this.state.loading && this.state.taskGroupRecords.length === 0 ?
                        <ListGroupItem disabled key="no-entries">
                            Er zijn nog geen taakverdelingen.
                        </ListGroupItem>
                    : null}
                    {this.props.tasks.length > 0 ?
                        <ListGroupItem onClick={this.createTaskGroupRecord} key="create">
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