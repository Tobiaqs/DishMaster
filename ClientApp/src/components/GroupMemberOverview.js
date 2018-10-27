import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem, Glyphicon } from 'react-bootstrap';
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
                    this.props.reload();
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
                {this.props.groupMembers.map(groupMember => 
                    <ListGroupItem key={groupMember.id} onClick={() => this.goToGroupMember(groupMember)}>
                        {groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName} ({Math.round(groupMember.score * 10) / 10})
                    </ListGroupItem>
                )}
                {this.props.groupRoles.administrator ?
                    <div>
                        <ListGroupItem onClick={this.linkGroupMember}>
                            <Glyphicon glyph="plus" /> <i>Nieuw groepslid koppelen&#8230;</i>
                        </ListGroupItem>
                        <ListGroupItem onClick={this.createAnonymousGroupMember}>
                            <Glyphicon glyph="plus" /> <i>Nieuw anoniem groepslid aanmaken&#8230;</i>
                        </ListGroupItem>
                    </div>
                : null}
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
            {this.state.selectedGroupMemberId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/groupMember/' + this.state.selectedGroupMemberId} push={true} /> : null}
            {this.state.linkGroupMember ? <Redirect to={'/group/' + this.props.match.params.groupId + '/link-group-member'} push={true} /> : null}
        </div>;
    }
}