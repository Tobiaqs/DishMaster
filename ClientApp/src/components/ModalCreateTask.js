import React, { Component } from 'react';

import { Checkbox, FormGroup, FormControl, ControlLabel, Modal, Button } from 'react-bootstrap';

export class ModalCreateTask extends Component {
    constructor(props) {
        super(props);

        this.state = {
            name: '',
            bounty: '0',
            isNeutral: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleKeyPress = this.handleKeyPress.bind(this);
        this.handleCreateTask = this.handleCreateTask.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (!this.props.creatingTask && (this.state.name.length > 0 || this.state.bounty !== '0' || this.state.isNeutral)) {
            this.setState({ name: '', bounty: '0', isNeutral: false });
        }
    }

    handleChange(event) {
        switch (event.target.id) {
            case 'name': this.setState({ name: event.target.value }); break;
            case 'bounty': this.setState({ bounty: event.target.value }); break;
            case 'isNeutral': this.setState({ isNeutral: event.target.checked }); break;
            default: break;
        }
    }

    handleKeyPress(event) {
        if (event.key === 'Enter') {
            this.handleCreateTask();
        }
    }

    handleCreateTask() {
        this.props.onCreateTask(this.state.name, this.state.isNeutral, this.state.bounty);
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.creatingTask} onHide={this.handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Taak aanmaken</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <FormGroup>
                    <ControlLabel htmlFor="name">Geef deze taak een naam, bijv. afwassen:</ControlLabel>
                    <FormControl autoFocus value={this.state.name} type="text" id="name" placeholder="Vul hier de naam van de taak in" onChange={this.handleChange} onKeyPress={this.handleKeyPress} />
                </FormGroup>
                <FormGroup>
                    <ControlLabel htmlFor="isNeutral">Is deze taak score-neutraal?</ControlLabel>
                    <Checkbox id="isNeutral" checked={this.state.isNeutral} onChange={this.handleChange} />
                </FormGroup>
                {this.state.isNeutral ? null : <FormGroup>
                        <ControlLabel htmlFor="bounty">Wat is de opbrengst van deze taak in punten?</ControlLabel>
                        <FormControl type="number" value={this.state.bounty} id="bounty" placeholder="Vul hier de opbrengst van deze taak in" onChange={this.handleChange} onKeyPress={this.handleKeyPress} />
                    </FormGroup>
                }
            </Modal.Body>
            <Modal.Footer>
                <Button bsStyle="primary" onClick={this.handleCreateTask}>Aanmaken</Button>
                <Button onClick={this.handleClose}>Sluiten</Button>
            </Modal.Footer>
        </Modal>;
    }
}