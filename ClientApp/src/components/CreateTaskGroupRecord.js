import React, { Component } from 'react';
import { Redirect } from 'react-router';
import { Button, ListGroup, ListGroupItem } from 'react-bootstrap';
import { Api } from '../Api';

export class CreateTaskGroupRecord extends Component {
    constructor(props) {
        super(props);

        this.state = {
            group: null,
            presentGroupMemberIds: null,
            selectedTaskGroupRecordId: null
        };

        this.toggle = this.toggle.bind(this);
        this.createTaskGroupRecord = this.createTaskGroupRecord.bind(this);
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.groupId !== this.props.match.params.groupId) {
            this.setState({ group: null });
            this.fetch();
        }
    }

    fetch() {
        Api.getInstance().Group.Get({ groupId: this.props.match.params.groupId }).then(result => {
            this.setState({
                group: result.payload,
                presentGroupMemberIds: result.payload.groupMembers.map(groupMember => groupMember.id)
            });
        });
    }

    toggle(groupMember) {
        let newList;
        if (this.state.presentGroupMemberIds.indexOf(groupMember.id) !== -1) {
            newList = this.state.presentGroupMemberIds.filter(groupMemberId => groupMemberId !== groupMember.id);
        } else {
            newList = this.state.presentGroupMemberIds.concat([groupMember.id]);
        }
        this.setState({ presentGroupMemberIds: newList })
    }

    createTaskGroupRecord() {
        Api.getInstance().TaskGroupRecord.Create({
            taskGroupId: this.props.match.params.taskGroupId,
            presentGroupMemberIds: this.state.presentGroupMemberIds
        }).then(result => {
            if (result.succeeded) {
                this.setState({ selectedTaskGroupRecordId: result.taskGroupRecordId });
            } else {
                alert("Failed!");
            }
        });
    }

    render() {
        return <div>
            {this.state.group ? <div>
                <h1>Aanwezigen</h1>
                <p>Selecteer de mensen die er zijn.</p>
                <ListGroup>
                    {this.state.group.groupMembers.map(groupMember => 
                        <ListGroupItem bsStyle={this.state.presentGroupMemberIds.indexOf(groupMember.id) !== -1 ? 'info' : null} key={groupMember.id} onClick={() => this.toggle(groupMember)}>
                            {groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName}
                        </ListGroupItem>
                    )}
                </ListGroup>
                <Button bsStyle="primary" disabled={this.state.presentGroupMemberIds.length === 0} block onClick={this.createTaskGroupRecord}>Verdeling maken</Button>
            </div> : <h1>Laden&#8230;</h1>}
            {this.state.selectedTaskGroupRecordId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/taskGroupRecord/' + this.state.selectedTaskGroupRecordId} push={false} /> : null}
        </div>;
    }
}