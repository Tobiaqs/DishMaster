import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Badge, ListGroup, ListGroupItem } from 'react-bootstrap';
import { Api } from '../Api';
import { ModalConfirm } from './ModalConfirm';

export class GroupMember extends Component {
    constructor(props) {
        super(props);

        this.state = {
            groupMember: null,
            groupRoles: null,
            deletingGroupMember: false,
            groupMemberDeleted: false
        };
        
        this.deleteGroupMember = this.deleteGroupMember.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalConfirmed = this.onModalConfirmed.bind(this);
        this.demoteGroupMember = this.demoteGroupMember.bind(this);
        this.promoteGroupMember = this.promoteGroupMember.bind(this);
        this.setGroupMemberAbsentByDefault = this.setGroupMemberAbsentByDefault.bind(this);
        this.setGroupMemberPresentByDefault = this.setGroupMemberPresentByDefault.bind(this);
    }

    fetch() {
        Api.getInstance().Group.GetGroupMember({ groupMemberId: this.props.match.params.groupMemberId }).then(result => {
            this.setState({ groupMember: result.payload });
            return Api.getInstance().Group.GetGroupRoles({ groupId: this.props.match.params.groupId });
        }).then(result => {
            this.setState({ groupRoles: result.payload });
        });
    }

    reload() {
        this.setState({ groupMember: null, groupRoles: null });
        this.fetch();
    }
    
    onModalHide() {
        this.setState({ deletingGroupMember: false });
    }

    onModalConfirmed() {
        Api.getInstance().Group.DeleteGroupMember({ groupMemberId: this.props.match.params.groupMemberId }).then(result => {
            this.onModalHide();
            if (result.succeeded) {
                this.setState({ groupMemberDeleted: true });
            } else {
                alert("Failed!");
            }
        });
    }

    deleteGroupMember() {
        this.setState({ deletingGroupMember: true });
    }

    promoteGroupMember() {
        Api.getInstance().Group.PromoteGroupMember({ groupMemberId: this.props.match.params.groupMemberId }).then(result => {
            if (result.succeeded) {
                this.reload();
            } else {
                alert("Failed!");
            }
        });
    }

    demoteGroupMember() {
        Api.getInstance().Group.DemoteGroupMember({ groupMemberId: this.props.match.params.groupMemberId }).then(result => {
            if (result.succeeded) {
                this.reload();
            } else {
                alert("Failed!");
            }
        });
    }

    setGroupMemberAbsentByDefault() {
        Api.getInstance().Group.UpdateGroupMember({
            groupMemberId: this.props.match.params.groupMemberId,
            absentByDefault: true
        }).then(result => {
            if (result.succeeded) {
                this.reload();
            } else {
                alert("Failed!");
            }
        });
    }

    setGroupMemberPresentByDefault() {
        Api.getInstance().Group.UpdateGroupMember({
            groupMemberId: this.props.match.params.groupMemberId,
            absentByDefault: false
        }).then(result => {
            if (result.succeeded) {
                this.reload();
            } else {
                alert("Failed!");
            }
        });
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.groupMemberId !== this.props.match.params.groupMemberId) {
            this.setState({ groupMember: null });
            this.fetch();
        }
    }

    getGroupMemberName() {
        return this.state.groupMember.isAnonymous ? this.state.groupMember.anonymousName : this.state.groupMember.fullName;
    }

    render() {
        return <div>
            {this.state.groupMember && this.state.groupRoles ? <div>
                <h1>{this.getGroupMemberName()}{this.state.groupMember.administrator ? <span> <Badge>admin</Badge></span> : null}</h1>
                <h4>Score</h4>
                <p>{this.getGroupMemberName()} heeft een score van {Math.round(this.state.groupMember.score * 10) / 10} {Math.round(this.state.groupMember.score * 10) / 10 === 1 ? "punt" : "punten"}.</p>
                {this.state.groupMember.id !== this.state.groupRoles.groupMemberId && this.state.groupRoles.administrator ?
                    <div>
                        <h4>Administratie</h4>
                        <ListGroup>
                            {this.state.groupMember.isAnonymous ? null :
                                <div>
                                    {this.state.groupMember.administrator ?
                                        <ListGroupItem onClick={this.demoteGroupMember}><i>Dit groepslid beheerdersrechten ontnemen&#8230;</i></ListGroupItem>
                                        : <ListGroupItem onClick={this.promoteGroupMember}><i>Dit groepslid beheerdersrechten geven&#8230;</i></ListGroupItem>
                                    }
                                    {this.state.groupMember.absentByDefault ?
                                        <ListGroupItem onClick={this.setGroupMemberPresentByDefault}><i>Dit groepslid standaard aanwezig maken&#8230;</i></ListGroupItem>
                                        : <ListGroupItem onClick={this.setGroupMemberAbsentByDefault}><i>Dit groepslid standaard afwezig maken&#8230;</i></ListGroupItem>
                                    }
                                </div>
                            }
                            <ListGroupItem onClick={this.deleteGroupMember}><i>Dit groepslid verwijderen&#8230;</i></ListGroupItem>
                        </ListGroup>
                    </div>
                : null}
            </div> : <h1>Laden&#8230;</h1>}
            <ModalConfirm
                show={this.state.deletingGroupMember}
                message="Weet je zeker dat je dit groepslid wilt verwijderen?"
                onHide={this.onModalHide}
                onConfirmed={this.onModalConfirmed} />
            {this.state.groupMemberDeleted ? <Redirect to={'/group/' + this.props.match.params.groupId} push={false} /> : null}
        </div>;
    }
}