import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem, Modal } from 'react-bootstrap';
import { Tools } from './Tools';

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
        
        if (groupMember) {
            Api.getInstance().TaskGroupRecord.AssignTask({
                groupMemberId: groupMember.id,
                taskId: assignedTask.taskId,
                taskGroupRecordId: this.props.taskGroupRecord.id
            }).then(result => {
                if (result.succeeded) {
                    this.props.onHide();
                    this.props.reload();
                } else {
                    alert("Failed!");
                }
            });
        } else if (assignedTask.groupMemberId) {
            // No new group member, but an old group member?
            // Get rid of it.
            Api.getInstance().TaskGroupRecord.UnassignTask({
                taskId: assignedTask.taskId,
                taskGroupRecordId: this.props.taskGroupRecord.id
            }).then(result => {
                if (result) {
                    this.props.onHide();
                    this.props.reload();
                } else {
                    alert("Failed!");
                }
            })
        } else {
            // No new group member and no old group member?
            // Then just hide and don't reload.
            this.props.onHide();
        }
    }

    handleClose() {
        this.props.onHide();
    }

    getPendingBounty(groupMember) {
        let sum = 0;
        let hasNeutral = false;

        this.props.taskGroupRecord.assignedTasks.forEach(assignedTask => {
            if (assignedTask.groupMemberId === groupMember.id) {
                const task = this.props.taskGroup.tasks.find(task => task.id === assignedTask.taskId);
                sum += task.bounty;
                if (task.isNeutral) {
                    hasNeutral = true;
                }
            }
        });

        if (hasNeutral) {
            if (sum === 0) {
                return <i>Δμ</i>;
            } else {
                return <span>{sum} + <i>Δμ</i></span>;
            }
        } else {
            return sum;
        }
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
                        <p>De score is aangegeven in de vorm (huidig aantal punten + toegewezen aantal punten).</p>
                        <ListGroup>
                            {this.props.taskGroupRecord.presentGroupMembersIds
                                .filter(id => id !== null)
                                .map(id => this.props.group.groupMembers.find(groupMember => groupMember.id === id))
                                .map(groupMember => <ListGroupItem
                                    key={groupMember.id}
                                    active={this.props.assignedTask.groupMemberId === groupMember.id}
                                    onClick={() => this.selectGroupMember(groupMember)}>
                                    {Tools.getGroupMemberNameDirect(groupMember)} ({Tools.roundScore(groupMember.score)} + {this.getPendingBounty(groupMember)})
                                </ListGroupItem>)
                            }
                            <ListGroupItem
                                onClick={() => this.selectGroupMember()}
                                active={!this.props.assignedTask.groupMemberId}>
                                    <i>Niemand</i>
                                </ListGroupItem>
                        </ListGroup>
                    </Modal.Body>
                </div>
            : null}
        </Modal>;
    }
}