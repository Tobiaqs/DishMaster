import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalCreateTask } from './ModalCreateTask';
import { FaPlus } from 'react-icons/fa';

export class TaskOverview extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedTaskId: null,
            creatingTask: false
        };

        this.addTask = this.addTask.bind(this);
        this.onModalCreateTask = this.onModalCreateTask.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.showTask = this.showTask.bind(this);
    }

    addTask() {
        this.setState({ creatingTask: true });
    }

    showTask(task) {
        this.setState({ selectedTaskId: task.id });
    }

    onModalCreateTask(values) {
        if (values.isNeutral) {
            values.bounty = 0;
        }
        values.taskGroupId = this.props.match.params.taskGroupId;
        Api.getInstance().Task.Create(values).then(result => {
            this.onModalHide();

            if (result.succeeded) {
                this.props.reload();
            } else {
                alert("Failed!");
            }
        });
    }

    onModalHide() {
        this.setState({ creatingTask: false });
    }

    render() {
        return <div>
                <h4>Taken</h4>
                <ListGroup>
                    {this.props.tasks.map(task => <ListGroupItem action onClick={() => this.showTask(task)} key={task.id}>{task.name}</ListGroupItem>)}
                    {this.props.tasks.length === 0 ?
                        <ListGroupItem action disabled key="none">
                            Er zijn nog geen taken.
                        </ListGroupItem>
                    : null}
                    {this.props.groupRoles.administrator ?
                        <ListGroupItem action onClick={this.addTask}><FaPlus/> <i>Taak toevoegen&#8230;</i></ListGroupItem>
                    : null}
                </ListGroup>
            {this.state.selectedTaskId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/task/' + this.state.selectedTaskId} push={true} /> : null}
            <ModalCreateTask
                creatingTask={this.state.creatingTask}
                onCreateTask={this.onModalCreateTask}
                onHide={this.onModalHide} />
        </div>;
    }
}