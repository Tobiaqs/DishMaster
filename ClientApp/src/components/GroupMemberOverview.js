import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalCreateSimple } from './ModalCreateSimple';

export class GroupMemberOverview extends Component {
    constructor(props) {
        super(props);

        this.state = {
            group: null,
            selectedGroupMemberId: null,
            creatingAnonymousGroupMember: false,
            linkGroupMember: false
        };

        this.linkGroupMember = this.linkGroupMember.bind(this);
        this.createAnonymousGroupMember = this.createAnonymousGroupMember.bind(this);
        this.goToGroupMember = this.goToGroupMember.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalCreateSimpleEntity = this.onModalCreateSimpleEntity.bind(this);
    }

    linkGroupMember() {
        this.setState({ linkGroupMember: true });
    }

    createAnonymousGroupMember() {
        this.setState({ creatingAnonymousGroupMember: true });
    }

    goToGroupMember(groupMember) {
        this.setState({ selectedGroupMemberId: groupMember.id });
    }

    onModalCreateSimpleEntity(name) {
        // todo more validation?
        if (name.length > 0) {
            Api.getInstance().Group.AddAnonymousGroupMember({
                anonymousName: name,
                groupId: this.props.match.params.groupId
            }).then(result => {
                this.onModalHide();
                if (result.succeeded) {
                    // don't immediately go to the new group member page
                    //this.setState({ selectedGroupMemberId: result.groupMemberId });
                    // rather refresh
                    this.fetch();
                } else {
                    alert("Failed!");
                }
            });
        }
    }

    onModalHide() {
        this.setState({ creatingAnonymousGroupMember: false });
    }

    renderGroupMembers() {
        return <div>
            <h4>Groepsleden</h4>
            <ListGroup>
                {this.props.group.groupMembers.map(groupMember => 
                    <ListGroupItem key={groupMember.id} onClick={() => this.goToGroupMember(groupMember)}>
                        {groupMember.fullName || groupMember.anonymousName}
                    </ListGroupItem>
                )}
                <ListGroupItem onClick={this.linkGroupMember}>
                    <i>Nieuw groepslid koppelen&#8230;</i>
                </ListGroupItem>
                <ListGroupItem onClick={this.createAnonymousGroupMember}>
                    <i>Nieuw anoniem groepslid aanmaken&#8230;</i>
                </ListGroupItem>
            </ListGroup>
        </div>;
    }

    render() {
        return <div>
            {this.renderGroupMembers()}
            <ModalCreateSimple
                onHide={this.onModalHide}
                onCreateSimpleEntity={this.onModalCreateSimpleEntity}
                creatingAnonymousGroupMember={this.state.creatingAnonymousGroupMember}
                creatingTaskGroup={this.state.creatingTaskGroup} />
            {this.state.selectedGroupMemberId ? <Redirect to={'/group/' + this.props.group.id + '/groupMember/' + this.state.selectedGroupMemberId} push={true} /> : null}
            {this.state.linkGroupMember ? <Redirect to={'/group/' + this.props.group.id + '/link-group-member'} push={true} /> : null}
        </div>;
    }
}