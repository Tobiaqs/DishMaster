import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem, Glyphicon } from 'react-bootstrap';
import { ModalCreateSimple } from './ModalCreateSimple';

export class TaskGroupOverview extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedTaskGroupId: null,
            creatingTaskGroup: false
        };

        this.createTaskGroup = this.createTaskGroup.bind(this);
        this.goToTaskGroup = this.goToTaskGroup.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalCreateSimpleEntity = this.onModalCreateSimpleEntity.bind(this);
    }

    createTaskGroup() {
        this.setState({ creatingTaskGroup: true });
    }

    goToTaskGroup(taskGroup) {
        this.setState({ selectedTaskGroupId: taskGroup.id });
    }

    onModalCreateSimpleEntity(name) {
        // todo more validation?
        if (name.length > 0) {
            Api.getInstance().TaskGroup.Create({
                name: name,
                groupId: this.props.match.params.groupId
            }).then(result => {
                this.onModalHide();
                if (result.succeeded) {
                    this.setState({ selectedTaskGroupId: result.taskGroupId });
                } else {
                    alert("Failed!");
                }
            });
        }
    }

    onModalHide() {
        this.setState({ creatingTaskGroup: false });
    }

    renderTaskGroups() {
        return <div>
            <h4>Taakgroepen</h4>
            <ListGroup>
                {this.props.taskGroups.map(taskGroup => 
                    <ListGroupItem key={taskGroup.id} onClick={() => this.goToTaskGroup(taskGroup)}>
                        {taskGroup.name}
                    </ListGroupItem>
                )}
                {this.props.taskGroups.length === 0 ?
                    <ListGroupItem disabled key="none">
                        Er zijn nog geen taakgroepen.
                    </ListGroupItem>
                : null}
                {this.props.groupRoles.administrator ?
                    <ListGroupItem onClick={this.createTaskGroup}>
                        <Glyphicon glyph="plus" /> <i>Nieuwe taakgroep aanmaken&#8230;</i>
                    </ListGroupItem>
                : null}
            </ListGroup>
        </div>;
    }

    render() {
        return <div>
            {this.renderTaskGroups()}
            {this.state.selectedTaskGroupId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.state.selectedTaskGroupId} push={true} /> : null}
            <ModalCreateSimple
                    onHide={this.onModalHide}
                    onCreateSimpleEntity={this.onModalCreateSimpleEntity}
                    creatingTaskGroup={this.state.creatingTaskGroup} />
        </div>;
    }
}