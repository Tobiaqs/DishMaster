import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api, AuthContext } from '../Api';
import { Badge, ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalCreateSimple } from './ModalCreateSimple';
import { ModalEditSimple } from './ModalEditSimple';
import { GroupListContext } from './NavMenu';
import { ModalConfirm } from './ModalConfirm';
import { TaskGroupOverview } from './TaskGroupOverview';
import { GroupMemberOverview } from './GroupMemberOverview';

export class Group extends Component {
    constructor(props) {
        super(props);

        this.state = {
            group: null,
            taskGroups: null,
            editingGroup: false,
            groupDeleted: false,
            leavingGroup: false
        };

        this.editGroup = this.editGroup.bind(this);
        this.leaveGroup = this.leaveGroup.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalLeave = this.onModalLeave.bind(this);
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

    onModalHide() {
        this.setState({
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
            <h1>{this.state.group.name} <Badge>groep</Badge></h1>
            <TaskGroupOverview taskGroups={this.state.taskGroups} group={this.state.group} />
            <GroupMemberOverview group={this.state.group} />

            <h4>Administratie</h4>
            <ListGroup>
                <ListGroupItem onClick={this.editGroup}><i>Deze groep hernoemen of verwijderen&#8230;</i></ListGroupItem>
                <ListGroupItem onClick={this.leaveGroup}><i>Deze groep verlaten&#8230;</i></ListGroupItem>
            </ListGroup>
        </div>;
    }

    render() {
        return <div>
            {this.state.group && this.state.taskGroups ? this.renderGroup() : <h1>Loading...</h1>}
            <GroupListContext.Consumer>
                {groupList => <span>
                    <ModalEditSimple
                        onHide={this.onModalHide}
                        originalName={this.state.group ? this.state.group.name : null}
                        onRenameSimpleEntity={(name) => this.onModalRenameSimpleEntity(name, groupList)}
                        onDeleteSimpleEntity={() => this.onModalDeleteSimpleEntity(groupList)}
                        editingGroup={this.state.editingGroup} />
                    <ModalConfirm
                        show={this.state.leavingGroup}
                        message="Weet je zeker dat je deze groep wilt verlaten?"
                        onHide={this.onModalHide}
                        onConfirmed={() => this.onModalLeave(groupList)}
                        />
                </span>}
            </GroupListContext.Consumer>
            {this.state.linkGroupMember ? <Redirect to={'/group/' + this.props.match.params.groupId + '/link-group-member'} push={true} /> : null}
            {this.state.groupDeleted ? <Redirect to="/" push={false} /> : null}
        </div>;
    }
}