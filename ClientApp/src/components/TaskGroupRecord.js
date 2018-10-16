import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { ListGroup, ListGroupItem } from 'react-bootstrap';
import { Api } from '../Api';
import { ModalConfirm } from './ModalConfirm';

export class TaskGroupRecord extends Component {
    constructor(props) {
        super(props);
        
        this.state = {
            taskGroupRecord: null, // for mapping
            taskGroup: null, // for task names
            group: null, // for group member names,
            deletingTaskGroupRecord: false,
            taskGroupRecordDeleted: false
        };

        this.getTaskName = this.getTaskName.bind(this);
        this.getGroupMemberName = this.getGroupMemberName.bind(this);
        this.finalizeTaskGroupRecord = this.finalizeTaskGroupRecord.bind(this);
        this.deleteTaskGroupRecord = this.deleteTaskGroupRecord.bind(this);
        this.finalizeTaskGroupRecord = this.finalizeTaskGroupRecord.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onDeleteTaskGroupRecord = this.onDeleteTaskGroupRecord.bind(this);
        this.onFinalizeTaskGroupRecord = this.onFinalizeTaskGroupRecord.bind(this);
    }

    fetch() {
        Api.getInstance().Group.Get({ groupId: this.props.match.params.groupId }).then(result => {
            this.setState({ group: result.payload });
            return Api.getInstance().TaskGroup.Get({ taskGroupId: this.props.match.params.taskGroupId });
        }).then(result => {
            this.setState({ taskGroup: result.payload });
            return Api.getInstance().TaskGroupRecord.Get({ taskGroupRecordId: this.props.match.params.taskGroupRecordId });
        }).then(result => {
            this.setState({ taskGroupRecord: result.payload });
        });
    }

    reload() {
        this.setState({
            taskGroupRecord: null,
            taskGroup: null,
            group: null
        });
        this.fetch();
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.taskGroupRecordId !== this.props.match.params.taskGroupRecordId ||
            prevProps.match.params.taskGroupId !== this.props.match.params.taskGroupId ||
            prevProps.match.params.groupId !== this.props.match.params.groupId) {
            this.reload();
        }
    }

    finalizeTaskGroupRecord() {
        this.setState({ finalizingTaskGroupRecord: true });
    }

    deleteTaskGroupRecord() {
        this.setState({ deletingTaskGroupRecord: true });
    }

    onModalHide() {
        this.setState({
            finalizingTaskGroupRecord: false,
            deletingTaskGroupRecord: false
        });
    }

    onDeleteTaskGroupRecord() {
        Api.getInstance().TaskGroupRecord.Delete({ taskGroupRecordId: this.props.match.params.taskGroupRecordId }).then(result => {
            this.onModalHide();
            if (result.succeeded) {
                this.setState({ taskGroupRecordDeleted: true });
            } else {
                alert("Failed!");
            }
        });
    }

    onFinalizeTaskGroupRecord() {
        Api.getInstance().TaskGroupRecord.Finalize({ taskGroupRecordId: this.props.match.params.taskGroupRecordId }).then(result => {
            this.onModalHide();
            if (result.succeeded) {
                this.reload();
            } else {
                alert("Failed!");
            }
        });
    }

    getTaskName(taskId) {
        const task = this.state.taskGroup.tasks.find(task => task.id === taskId);
        return task != null ? task.name : "<verwijderd>";
    }

    getGroupMemberName(groupMemberId) {
        const groupMember = this.state.group.groupMembers.find(groupMember => groupMember.id === groupMemberId);
        return groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName;
    }

    render() {
        return <div>
            {this.state.taskGroupRecord ? <div>
                <h1>Taakverdeling</h1>
                <h4>Taken</h4>
                <ListGroup>
                    {this.state.taskGroupRecord.assignedTasks.map(assignedTask => {
                        return <ListGroupItem key={assignedTask.randomId}>
                            {this.getTaskName(assignedTask.taskId) + ": " + this.getGroupMemberName(assignedTask.groupMemberId)} ({assignedTask.thenBounty})
                        </ListGroupItem>;
                    })}
                </ListGroup>
                    {this.state.taskGroupRecord.finalized ? null :
                        <div>
                            <h4>Administratie</h4>
                            <ListGroup>
                                <ListGroupItem onClick={this.finalizeTaskGroupRecord}><i>Deze verdeling definitief maken&#8230;</i></ListGroupItem>
                                <ListGroupItem onClick={this.deleteTaskGroupRecord}><i>Deze verdeling verwijderen&#8230;</i></ListGroupItem>
                            </ListGroup>
                        </div>
                    }
            </div> : <div>
                <h1>Laden&#8230;</h1>
            </div>}
            <ModalConfirm
                show={this.state.deletingTaskGroupRecord}
                message="Weet je zeker dat je deze verdeling wil verwijderen?"
                onHide={this.onModalHide}
                onConfirmed={this.onDeleteTaskGroupRecord} />
            <ModalConfirm
                show={this.state.finalizingTaskGroupRecord}
                message="Weet je zeker dat je deze verdeling definitief wil maken?"
                onHide={this.onModalHide}
                onConfirmed={this.onFinalizeTaskGroupRecord} />
            {this.state.taskGroupRecordDeleted ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId} push={false} /> : null}
        </div>;
    }
}