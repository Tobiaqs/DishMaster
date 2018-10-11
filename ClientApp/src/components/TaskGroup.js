import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalCreateTask } from './ModalCreateTask';

export class TaskGroup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskGroup: null,
            selectedTaskId: null,
            creatingTask: false
        };

        this.addTask = this.addTask.bind(this);
        this.deleteTaskGroup = this.deleteTaskGroup.bind(this);
        this.onModalCreateTask = this.onModalCreateTask.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.showTask = this.showTask.bind(this);
    }

    fetch() {
        Api.getInstance().TaskGroup.Get({ taskGroupId: this.props.match.params.taskGroupId }).then(result => {
            this.setState({ taskGroup: result.payload })
        });
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.taskGroupId !== this.props.match.params.taskGroupId) {
            this.setState({ taskGroup: null });
            this.fetch();
        }
    }

    addTask() {
        this.setState({ creatingTask: true });
    }

    deleteTaskGroup() {

    }

    showTask(task) {
        this.setState({ selectedTaskId: task.id });
    }

    onModalCreateTask(name, isNeutral, bounty) {
        const bountyNum = bounty * 1;

        if (name.length === 0 || isNaN(bountyNum) || Math.ceil(bountyNum) !== Math.floor(bountyNum)) {
            alert("Failed!");
            return;
        }

        Api.getInstance().Task.Create({
            name: name,
            isNeutral: isNeutral,
            bounty: bountyNum,
            taskGroupId: this.state.taskGroup.id
        }).then(result => {
            this.onModalHide();

            if (result.succeeded) {
                this.setState({ taskGroup: null });
                this.fetch();
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
            {this.state.taskGroup ? <div>
                <h1>{this.state.taskGroup.name}</h1>
                <h4>Taken</h4>
                <ListGroup>
                    {this.state.taskGroup.tasks.map(task => <ListGroupItem onClick={() => this.showTask(task)}>{task.name}</ListGroupItem>)}
                    <ListGroupItem onClick={this.addTask}><i>Taak toevoegen&#8230;</i></ListGroupItem>
                </ListGroup>
                <h4>Administratie</h4>
                <ListGroup>
                    <ListGroupItem onClick={this.deleteTaskGroup}><i>Deze taakgroep verwijderen&#8230;</i></ListGroupItem>
                </ListGroup>
            </div> : <h1>Laden&#8230;</h1>}
            {this.state.selectedTaskId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/task/' + this.state.selectedTaskId} push={true} /> : null}
            <ModalCreateTask
                creatingTask={this.state.creatingTask}
                onCreateTask={this.onModalCreateTask}
                onHide={this.onModalHide} />
        </div>;
    }
}