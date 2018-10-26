import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { Badge, ListGroup, ListGroupItem } from 'react-bootstrap';
import { ModalEditSimple } from './ModalEditSimple';
import { TaskOverview } from './TaskOverview';
import { TaskGroupRecordOverview } from './TaskGroupRecordOverview';

export class TaskGroup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskGroup: null,
            groupRoles: null,
            editingTaskGroup: false,
            taskGroupDeleted: false
        };

        this.onModalHide = this.onModalHide.bind(this);
        this.onModalRenameSimpleEntity = this.onModalRenameSimpleEntity.bind(this);
        this.onModalDeleteSimpleEntity = this.onModalDeleteSimpleEntity.bind(this);
        this.editTaskGroup = this.editTaskGroup.bind(this);
        this.reload = this.reload.bind(this);
    }

    fetch() {
        Api.getInstance().TaskGroup.Get({ taskGroupId: this.props.match.params.taskGroupId }).then(result => {
            this.setState({ taskGroup: result.payload })
            return Api.getInstance().Group.GetGroupRoles({ groupId: this.props.match.params.groupId });
        }).then(result => {
            this.setState({ groupRoles: result.payload });
        });
    }

    reload() {
        this.setState({ taskGroup: null });
        this.fetch();
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.taskGroupId !== this.props.match.params.taskGroupId) {
            this.reload();
        }
    }

    editTaskGroup() {
        this.setState({ editingTaskGroup: true });
    }
    
    onModalDeleteSimpleEntity() {
        Api.getInstance().TaskGroup.Delete({ taskGroupId: this.props.match.params.taskGroupId }).then(result => {
            this.onModalHide();
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
                    this.reload();
                } else {
                    alert("Failed!");
                }
            });
        }
    }

    onModalHide() {
        this.setState({ editingTaskGroup: false });
    }

    render() {
        return <div>
            {this.state.taskGroup && this.state.groupRoles ? <div>
                <h1>{this.state.taskGroup.name} <Badge>taakgroep</Badge></h1>
                <TaskOverview tasks={this.state.taskGroup.tasks} match={this.props.match} reload={this.reload} groupRoles={this.state.groupRoles}  />
                <TaskGroupRecordOverview taskGroupRecords={this.state.taskGroup.taskGroupRecords} match={this.props.match} />
                {this.state.groupRoles.administrator ?
                    <div>
                        <h4>Administratie</h4>
                        <ListGroup>
                            <ListGroupItem onClick={this.editTaskGroup}><i>Deze taakgroep hernoemen of verwijderen&#8230;</i></ListGroupItem>
                        </ListGroup>
                    </div>
                : null}
            </div> : <h1>Laden&#8230;</h1>}
            {this.state.taskGroupDeleted ? <Redirect to={'/group/' + this.props.match.params.groupId} push={false} /> : null}
            <ModalEditSimple
                originalName={this.state.taskGroup ? this.state.taskGroup.name : null}
                editingTaskGroup={this.state.editingTaskGroup}
                onRenameSimpleEntity={this.onModalRenameSimpleEntity}
                onDeleteSimpleEntity={this.onModalDeleteSimpleEntity}
                onHide={this.onModalHide} />
        </div>;
    }
}