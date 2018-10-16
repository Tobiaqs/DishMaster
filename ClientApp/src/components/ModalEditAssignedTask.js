import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem, Modal, Button } from 'react-bootstrap';

export class ModalEditAssignedTask extends Component {
    constructor(props) {
        super(props);

        this.state = {
            deleting: false
        };

        this.selectGroupMember = this.selectGroupMember.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    selectGroupMember(groupMember) {
        const { assignedTask } = this.props;
        let promise = Promise.resolve({ succeeded: true });

        // If necessary first unassign old group member from task
        if (assignedTask.groupMemberId) {
            promise = Api.getInstance().TaskGroupRecord.UnassignTask({
                taskId: assignedTask.taskId,
                taskGroupRecordId: this.props.taskGroupRecord.id
            });
        }

        // If necessary assign new group member to task
        if (groupMember) {
            promise.then(result => {
                if (result.succeeded) {
                    return Api.getInstance().TaskGroupRecord.AssignTask({
                        groupMemberId: groupMember.id,
                        taskId: assignedTask.taskId,
                        taskGroupRecordId: this.props.taskGroupRecord.id
                    });
                } else {
                    return { succeeded: false };
                }
            }).then(result => {
                if (result.succeeded) {
                    this.props.onHide();
                    this.props.reload();
                } else {
                    alert("Failed!");
                }
            });
        } else if (!assignedTask.groupMemberId) {
             // No new group member and no old group member?
             // Then just hide and don't reload.
            this.props.onHide();
        } else {
            // No old group member, but a new group member?
            // Hide and reload.
            this.props.onHide();
            this.props.reload();
        }
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={!!this.props.assignedTask} onHide={this.handleClose}>
            {this.props.assignedTask ? 
                <div>
                    <Modal.Header closeButton>
                        <Modal.Title>Toegewezene veranderen</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>Kies hieronder de nieuwe toegewezene.</p>
                        <ListGroup>
                            {this.props.taskGroupRecord.presentGroupMembersIds
                                .map(id => this.props.group.groupMembers.find(groupMember => groupMember.id === id))
                                .map(groupMember => <ListGroupItem
                                    key={groupMember.id}
                                    active={this.props.assignedTask.groupMemberId === groupMember.id}
                                    onClick={() => this.selectGroupMember(groupMember)}>
                                    {groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName}
                                </ListGroupItem>)
                            }
                            <ListGroupItem
                                onClick={() => this.selectGroupMember()}
                                active={!this.props.assignedTask.groupMemberId}>
                                    <i>Niemand</i>
                                </ListGroupItem>
                        </ListGroup>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={this.handleClose}>Sluiten</Button>
                    </Modal.Footer>
                </div>
            : null}
        </Modal>;
    }
}