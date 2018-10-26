import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { ListGroup, ListGroupItem, Table } from 'react-bootstrap';
import { Api } from '../Api';
import { ModalConfirm } from './ModalConfirm';
import { ModalEditAssignedTask } from './ModalEditAssignedTask';

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

        this.isTaskDeleted = this.isTaskDeleted.bind(this);
        this.getTaskName = this.getTaskName.bind(this);
        this.getTaskBounty = this.getTaskBounty.bind(this);
        this.getGroupMemberName = this.getGroupMemberName.bind(this);
        this.finalizeTaskGroupRecord = this.finalizeTaskGroupRecord.bind(this);
        this.deleteTaskGroupRecord = this.deleteTaskGroupRecord.bind(this);
        this.finalizeTaskGroupRecord = this.finalizeTaskGroupRecord.bind(this);
        this.editAssignedTask = this.editAssignedTask.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onDeleteTaskGroupRecord = this.onDeleteTaskGroupRecord.bind(this);
        this.onFinalizeTaskGroupRecord = this.onFinalizeTaskGroupRecord.bind(this);
        this.reload = this.reload.bind(this);
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

    editAssignedTask(assignedTask) {
        this.setState({ editingAssignedTask: assignedTask });
    }

    onModalHide() {
        this.setState({
            finalizingTaskGroupRecord: false,
            deletingTaskGroupRecord: false,
            editingAssignedTask: null
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

    isTaskDeleted(taskId) {
        const task = this.state.taskGroup.tasks.find(task => task.id === taskId);
        return !task;
    }

    getTaskName(taskId) {
        const task = this.state.taskGroup.tasks.find(task => task.id === taskId);
        return task != null ? task.name : <i>Verwijderd</i>;
    }

    getTaskBounty(taskId) {
        const task = this.state.taskGroup.tasks.find(task => task.id === taskId);
        return task != null ? task.bounty : <i>Onbekend</i>;
    }

    getGroupMemberName(groupMemberId) {
        const groupMember = this.state.group.groupMembers.find(groupMember => groupMember.id === groupMemberId);
        if (groupMember == null) {
            return <i>Niemand</i>;
        }
        return groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName;
    }

    renderTaskGroupRecord() {
        return <div>
            <h1>Taakverdeling</h1>
            <h4>Taken</h4>
            <Table striped bordered condensed hover>
                <thead>
                    <tr>
                        <th>Taak</th>
                        <th>Toegewezene</th>
                        <th>Punten</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.taskGroupRecord.assignedTasks.map(assignedTask => {
                        return <tr key={assignedTask.randomId} onClick={this.state.taskGroupRecord.finalized || this.isTaskDeleted(assignedTask.taskId) ? null : () => this.editAssignedTask(assignedTask)}>
                            <td>{this.getTaskName(assignedTask.taskId)}</td>
                            <td>{this.getGroupMemberName(assignedTask.groupMemberId)}</td>
                            <td>{
                                this.state.taskGroupRecord.finalized ?
                                    assignedTask.thenBounty :
                                    this.getTaskBounty(assignedTask.taskId)
                            }</td>
                        </tr>;
                    })}
                </tbody>
            </Table>
            {this.state.taskGroupRecord.finalized ? null :
                <div>
                    <h4>Administratie</h4>
                    <ListGroup>
                        <ListGroupItem onClick={this.finalizeTaskGroupRecord}><i>Deze verdeling definitief maken&#8230;</i></ListGroupItem>
                        <ListGroupItem onClick={this.deleteTaskGroupRecord}><i>Deze verdeling verwijderen&#8230;</i></ListGroupItem>
                    </ListGroup>
                </div>
            }
            <ModalEditAssignedTask assignedTask={this.state.editingAssignedTask}
                group={this.state.group}
                taskGroup={this.state.taskGroup}
                taskGroupRecord={this.state.taskGroupRecord}
                onHide={this.onModalHide}
                reload={this.reload} />
        </div>;
    }

    render() {
        return <div>
            {this.state.group && this.state.taskGroup && this.state.taskGroupRecord ? this.renderTaskGroupRecord() : <div>
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