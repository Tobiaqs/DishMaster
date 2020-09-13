import React, { Component } from 'react';
import { Redirect } from 'react-router';
import { Button, ListGroup, ListGroupItem } from 'react-bootstrap';
import { Api } from '../Api';

export class CreateTaskGroupRecord extends Component {
    constructor(props) {
        super(props);

        this.state = {
            group: null,
            presentGroupMembersIds: null,
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
                presentGroupMembersIds: result.payload.groupMembers
                    .filter(groupMember => !groupMember.absentByDefault)
                    .map(groupMember => groupMember.id)
            });
        });
    }

    toggle(groupMember) {
        let newList;
        if (this.state.presentGroupMembersIds.indexOf(groupMember.id) !== -1) {
            newList = this.state.presentGroupMembersIds.filter(groupMemberId => groupMemberId !== groupMember.id);
        } else {
            newList = this.state.presentGroupMembersIds.concat([groupMember.id]);
        }
        this.setState({ presentGroupMembersIds: newList })
    }

    createTaskGroupRecord() {
        Api.getInstance().TaskGroupRecord.Create({
            taskGroupId: this.props.match.params.taskGroupId,
            presentGroupMembersIds: this.state.presentGroupMembersIds
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
                    {this.state.group.groupMembers
                        .map(groupMember => {
                            return {
                                ...groupMember,
                                name: groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName
                            };
                        })
                        .sort((a, b) => {
                            a = a.name.toLowerCase();
                            b = b.name.toLowerCase();

                            if (a === b) {
                                return 0;
                            }
                            
                            return a > b ? 1 : -1;
                        }).map(groupMember => 
                            <ListGroupItem action active={this.state.presentGroupMembersIds.indexOf(groupMember.id) !== -1} key={groupMember.id} onClick={() => this.toggle(groupMember)}>
                                {groupMember.name}
                            </ListGroupItem>
                        )
                    }
                </ListGroup>
                <Button variant="primary" disabled={this.state.presentGroupMembersIds.length < 2} block onClick={this.createTaskGroupRecord}>Verdeling maken</Button>
            </div> : <h1>Laden&#8230;</h1>}
            {this.state.selectedTaskGroupRecordId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/taskGroupRecord/' + this.state.selectedTaskGroupRecordId} push={false} /> : null}
        </div>;
    }
}