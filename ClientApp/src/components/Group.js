import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api, AuthContext } from '../Api';
import { ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalCreateSimple } from './ModalCreateSimple';
import { ModalEditSimple } from './ModalEditSimple';
import { GroupListContext } from './NavMenu';
import { ModalConfirm } from './ModalConfirm';

export class Group extends Component {
    constructor(props) {
        super(props);

        this.state = {
            group: null,
            taskGroups: null,
            selectedTaskGroupId: null,
            selectedGroupMemberId: null,
            creatingAnonymousGroupMember: false,
            creatingTaskGroup: false,
            editingGroup: false,
            linkGroupMember: false,
            groupDeleted: false,
            leavingGroup: false
        };

        this.createTaskGroup = this.createTaskGroup.bind(this);
        this.createGroupMember = this.createGroupMember.bind(this);
        this.createAnonymousGroupMember = this.createAnonymousGroupMember.bind(this);
        this.goToTaskGroup = this.goToTaskGroup.bind(this);
        this.goToGroupMember = this.goToGroupMember.bind(this);
        this.editGroup = this.editGroup.bind(this);
        this.leaveGroup = this.leaveGroup.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalLeave = this.onModalLeave.bind(this);
        this.onModalCreateSimpleEntity = this.onModalCreateSimpleEntity.bind(this);
        this.onModalRenameSimpleEntity = this.onModalRenameSimpleEntity.bind(this);
        this.onModalDeleteSimpleEntity = this.onModalDeleteSimpleEntity.bind(this);
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.groupId !== this.props.match.params.groupId) {
            this.fetch();
        }
    }

    createTaskGroup() {
        this.setState({ creatingTaskGroup: true });
    }

    createGroupMember() {
        this.setState({ linkGroupMember: true });
    }

    createAnonymousGroupMember() {
        this.setState({ creatingAnonymousGroupMember: true });
    }

    goToTaskGroup(taskGroup) {
        this.setState({ selectedTaskGroupId: taskGroup.id });
    }

    goToGroupMember(groupMember) {
        this.setState({ selectedGroupMemberId: groupMember.id });
    }

    editGroup() {
        this.setState({ editingGroup: true });
    }

    leaveGroup() {
        this.setState({ leavingGroup: true });
    }

    onModalRenameSimpleEntity(name, groupList) {
        if (name.length > 0) {
            Api.getInstance().Group.Update({
                name: name,
                groupId: this.props.match.params.groupId
            }).then(result => {
                this.onModalHide();
                if (result.succeeded) {
                    groupList.setNeedsUpdate(true);
                    this.fetch();
                } else {
                    alert("Failed!");
                }
            });
        }
    }

    onModalDeleteSimpleEntity(groupList) {
        Api.getInstance().Group.Delete({
            groupId: this.props.match.params.groupId
        }).then(result => {
            this.onModalHide();

            if (result.succeeded) {
                groupList.setNeedsUpdate(true);
                this.setState({ groupDeleted: true });
            } else {
                alert("Failed!");
            }
        })
    }

    onModalCreateSimpleEntity(name, groupList) {
        // todo more validation?
        if (name.length > 0) {
            if (this.state.creatingTaskGroup) {
                Api.getInstance().TaskGroup.Create({
                    name: name,
                    groupId: this.props.match.params.groupId
                }).then(result => {
                    this.onModalHide();
                    if (result.succeeded) {
                        groupList.setNeedsUpdate(true);
                        this.setState({ selectedTaskGroupId: result.taskGroupId });
                    } else {
                        alert("Failed!");
                    }
                });
            } else if (this.state.creatingAnonymousGroupMember) {
                Api.getInstance().Group.AddAnonymousGroupMember({
                    anonymousName: name,
                    groupId: this.props.match.params.groupId
                }).then(result => {
                    this.onModalHide();
                    if (result.succeeded) {
                        groupList.setNeedsUpdate(true);
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
    }

    onModalHide() {
        this.setState({
            creatingTaskGroup: false,
            creatingAnonymousGroupMember: false,
            editingGroup: false,
            leavingGroup: false
        });
    }

    onModalLeave(groupList) {
        Api.getInstance().Group.LeaveGroup({ groupId: this.props.match.params.groupId }).then(result => {
            this.onModalHide();

            if (result.succeeded) {
                groupList.setNeedsUpdate(true);
                this.setState({ groupDeleted: true });
            } else {
                alert("Failed!");
            }
        });
    }

    fetch() {
        if (this.state.group && this.state.taskGroups) {
            this.setState({ group: null, taskGroups: null });
        }
        Api.getInstance().Group.Get({ groupId: this.props.match.params.groupId }).then(result => {
            this.setState({ group: result.payload });
            return Api.getInstance().TaskGroup.List({ groupId: this.props.match.params.groupId });
        }).then((result) => {
            this.setState({ taskGroups: result.payload });
        });
    }

    renderGroup() {
        return <div>
            <h1>{this.state.group.name}</h1>
            <h4>Taakgroepen</h4>
            <ListGroup>
                {this.state.taskGroups.map(taskGroup => 
                    <ListGroupItem key={taskGroup.id} onClick={() => this.goToTaskGroup(taskGroup)}>
                        {taskGroup.name}
                    </ListGroupItem>
                )}
                <ListGroupItem onClick={this.createTaskGroup}>
                    <i>Nieuwe taakgroep aanmaken&#8230;</i>
                </ListGroupItem>
            </ListGroup>
            <h4>Groepsleden</h4>
            <ListGroup>
                {this.state.group.groupMembers.map(groupMember => 
                    <ListGroupItem key={groupMember.id} onClick={() => this.goToGroupMember(groupMember)}>
                        {groupMember.fullName || groupMember.anonymousName}
                    </ListGroupItem>
                )}
                <ListGroupItem onClick={this.createGroupMember}>
                    <i>Nieuw groepslid koppelen&#8230;</i>
                </ListGroupItem>
                <ListGroupItem onClick={this.createAnonymousGroupMember}>
                    <i>Nieuw anoniem groepslid aanmaken&#8230;</i>
                </ListGroupItem>
            </ListGroup>

            <h4>Administratie</h4>
            <ListGroup>
                <ListGroupItem onClick={this.editGroup}><i>Deze groep hernoemen of verwijderen&#8230;</i></ListGroupItem>
                <ListGroupItem onClick={this.leaveGroup}><i>Deze groep verlaten&#8230;</i></ListGroupItem>
            </ListGroup>
        </div>;
    }

    render() {
        return [
            <AuthContext.Consumer key="main">
                {auth => {
                    if (this.state.group && this.state.taskGroups) {
                        return this.renderGroup();
                    } else {
                        return <h1>Loading...</h1>;
                    }
                }}
            </AuthContext.Consumer>,
            <GroupListContext.Consumer key="modals">
                {groupList => [
                    <ModalCreateSimple
                        key="modal1"
                        onHide={this.onModalHide}
                        onCreateSimpleEntity={(name) => this.onModalCreateSimpleEntity(name, groupList)}
                        creatingAnonymousGroupMember={this.state.creatingAnonymousGroupMember}
                        creatingTaskGroup={this.state.creatingTaskGroup} />,
                    <ModalEditSimple
                        key="modal2"
                        onHide={this.onModalHide}
                        originalName={this.state.group ? this.state.group.name : null}
                        onRenameSimpleEntity={(name) => this.onModalRenameSimpleEntity(name, groupList)}
                        onDeleteSimpleEntity={() => this.onModalDeleteSimpleEntity(groupList)}
                        editingGroup={this.state.editingGroup} />,
                    <ModalConfirm
                        key="modal3"
                        show={this.state.leavingGroup}
                        message="Weet je zeker dat je deze groep wilt verlaten?"
                        onHide={this.onModalHide}
                        onConfirmed={() => this.onModalLeave(groupList)}
                        />
                ]}
            </GroupListContext.Consumer>,
            this.state.selectedTaskGroupId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.state.selectedTaskGroupId} push={true} key="redirect1" /> : null,
            this.state.selectedGroupMemberId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/groupMember/' + this.state.selectedGroupMemberId} push={true} key="redirect2" /> : null,
            this.state.linkGroupMember ? <Redirect to={'/group/' + this.props.match.params.groupId + '/link-group-member'} push={true} key="redirect3" /> : null,
            this.state.groupDeleted ? <Redirect to="/" push={false} key="redirect4" /> : null
        ];
    }
}