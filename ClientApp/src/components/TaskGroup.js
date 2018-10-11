import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { Badge, ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalCreateTask } from './ModalCreateTask';
import { ModalEditSimple } from './ModalEditSimple';

export class TaskGroup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskGroup: null,
            selectedTaskId: null,
            creatingTask: false,
            editingTaskGroup: false,
            taskGroupDeleted: false
        };

        this.addTask = this.addTask.bind(this);
        this.editTaskGroup = this.editTaskGroup.bind(this);
        this.onModalCreateTask = this.onModalCreateTask.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalConfirmed = this.onModalConfirmed.bind(this);
        this.onModalRenameSimpleEntity = this.onModalRenameSimpleEntity.bind(this);
        this.onModalDeleteSimpleEntity = this.onModalDeleteSimpleEntity.bind(this);
        this.showTask = this.showTask.bind(this);
        this.createTaskRecord = this.createTaskRecord.bind(this);
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

    editTaskGroup() {
        this.setState({ editingTaskGroup: true });
    }

    showTask(task) {
        this.setState({ selectedTaskId: task.id });
    }

    createTaskRecord() {

    }
    
    onModalDeleteSimpleEntity() {
        Api.getInstance().TaskGroup.Delete({ taskGroupId: this.props.match.params.taskGroupId }).then(result => {
            if (result.succeeded) {
                this.setState({ taskGroupDeleted: true });
            } else {
                alert("Failed!");
            }
        })
    }

    onModalRenameSimpleEntity(name) {
        // todo more validation?
        if (name.length > 0) {
            Api.getInstance().TaskGroup.Update({
                taskGroupId: this.props.match.params.taskGroupId,
                name: name
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
    }

    onModalCreateTask(name, isNeutral, bounty) {
        const bountyNum = bounty * 1;

        // todo more validation?
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

    onModalConfirmed() {
        Api.getInstance().TaskGroup.Delete({ taskGroupId: this.props.match.params.taskGroupId }).then(result => {
            this.onModalHide();
            if (result.succeeded) {
                this.setState({ taskGroupDeleted: true });
            } else {
                alert("Failed!");
            }
        });
    }

    onModalHide() {
        this.setState({ creatingTask: false, editingTaskGroup: false });
    }

    render() {
        return <div>
            {this.state.taskGroup ? <div>
                <h1>{this.state.taskGroup.name} <Badge>taakgroep</Badge></h1>
                <h4>Taken</h4>
                <ListGroup>
                    {this.state.taskGroup.tasks.map(task => <ListGroupItem onClick={() => this.showTask(task)} key={task.id}>{task.name}</ListGroupItem>)}
                    <ListGroupItem onClick={this.addTask}><i>Taak toevoegen&#8230;</i></ListGroupItem>
                </ListGroup>
                <h4>Taakverdelingen</h4>
                <ListGroup>
                    {/*this.state.taskGroup.tasks.map(task => <ListGroupItem onClick={() => this.showTask(task)} key={task.id}>{task.name}</ListGroupItem>)*/}
                    <ListGroupItem onClick={this.createTaskRecord}><i>Maak een voorlopige taakverdeling&#8230;</i></ListGroupItem>
                </ListGroup>
                <h4>Administratie</h4>
                <ListGroup>
                    <ListGroupItem onClick={this.editTaskGroup}><i>Deze taakgroep hernoemen of verwijderen&#8230;</i></ListGroupItem>
                </ListGroup>
            </div> : <h1>Laden&#8230;</h1>}
            {this.state.selectedTaskId ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId + '/task/' + this.state.selectedTaskId} push={true} /> : null}
            {this.state.taskGroupDeleted ? <Redirect to={'/group/' + this.props.match.params.groupId} push={false} /> : null}
            <ModalCreateTask
                creatingTask={this.state.creatingTask}
                onCreateTask={this.onModalCreateTask}
                onHide={this.onModalHide} />
            <ModalEditSimple
                originalName={this.state.taskGroup ? this.state.taskGroup.name : null}
                editingTaskGroup={this.state.editingTaskGroup}
                onRenameSimpleEntity={this.onModalRenameSimpleEntity}
                onDeleteSimpleEntity={this.onModalDeleteSimpleEntity}
                onHide={this.onModalHide} />
        </div>;
    }
}