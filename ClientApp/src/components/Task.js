import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { Badge, ListGroup, ListGroupItem, Glyphicon } from 'react-bootstrap';
import { ModalConfirm } from './ModalConfirm';

export class Task extends Component {
    constructor(props) {
        super(props);

        this.state = {
            task: null,
            groupRoles: null,
            deletingTask: false,
            taskDeleted: false
        };

        this.deleteTask = this.deleteTask.bind(this);
        this.onModalHide = this.onModalHide.bind(this);
        this.onModalConfirmed = this.onModalConfirmed.bind(this);
    }

    fetch() {
        Api.getInstance().Task.Get({ taskId: this.props.match.params.taskId }).then(result => {
            this.setState({ task: result.payload });
            return Api.getInstance().Group.GetGroupRoles({ groupId: this.props.match.params.groupId });
        }).then(result => {
            this.setState({ groupRoles: result.payload });
        });
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.taskId !== this.props.match.params.taskId) {
            this.setState({ task: null });
            this.fetch();
        }
    }
    
    deleteTask() {
        this.setState({ deletingTask: true });
    }

    onModalHide() {
        this.setState({ deletingTask: false });
    }

    onModalConfirmed() {
        Api.getInstance().Task.Delete({ taskId: this.props.match.params.taskId }).then(result => {
            this.onModalHide();
            if (result.succeeded) {
                this.setState({ taskDeleted: true });
            } else {
                alert("Failed!");
            }
        })
    }

    render() {
        return <div>
            {this.state.task && this.state.groupRoles ? <div>
                <h1>{this.state.task.name} <Badge>taak</Badge> </h1>
                <h4>Details</h4>
                <p>{this.state.task.isNeutral ? "Deze taak is score-neutraal. Dit betekent dat men bij het vervullen van deze taak geen punten zal inhalen op de rest, maar ook niet zal achterblijven. Het puntensaldo schuift mee met het gemiddelde binnen de groep." : "Deze taak levert " + this.state.task.bounty + " " + (this.state.task.bounty === 1 ? "punt" : "punten") + " op."}</p>
                {this.state.groupRoles.administrator ?
                    <div>
                        <h4>Administratie</h4>
                        <ListGroup>
                            <ListGroupItem onClick={this.deleteTask}><Glyphicon glyph="trash" /> <i>Deze taak verwijderen&#8230;</i></ListGroupItem>
                        </ListGroup>
                    </div>
                : null}
            </div> : <h1>Laden&#8230;</h1>}
            <ModalConfirm
                show={this.state.deletingTask}
                message="Weet je zeker dat je deze taak wilt verwijderen?"
                onHide={this.onModalHide}
                onConfirmed={this.onModalConfirmed} />
            {this.state.taskDeleted ? <Redirect to={'/group/' + this.props.match.params.groupId + '/taskGroup/' + this.props.match.params.taskGroupId} push={false} /> : null}
        </div>;
    }
}