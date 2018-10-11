import React, { Component } from 'react';

import { FormGroup, FormControl, ControlLabel, Modal, Button } from 'react-bootstrap';

export class ModalCreateSimple extends Component {
    constructor(props) {
        super(props);

        this.state = {
            name: ''
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleKeyPress = this.handleKeyPress.bind(this);
        this.handleCreateSimpleEntity = this.handleCreateSimpleEntity.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (!this.props.creatingAnonymousGroupMember && !this.props.creatingGroup && !this.props.creatingTaskGroup && this.state.name.length > 0) {
            this.setState({ name: '' });
        }
    }

    handleChange(event) {
        const update = {};
        update[event.target.id] = event.target.value;
        this.setState(update);
    }

    handleKeyPress(event) {
        if (event.key === 'Enter') {
            this.handleCreateSimpleEntity();
        }
    }

    handleCreateSimpleEntity() {
        this.props.onCreateSimpleEntity(this.state.name);
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.creatingGroup || this.props.creatingTaskGroup || this.props.creatingAnonymousGroupMember} onHide={this.handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>
                    {this.props.creatingGroup ? "Groep aanmaken" : null}
                    {this.props.creatingTaskGroup ? "Taakgroep aanmaken" : null}
                    {this.props.creatingAnonymousGroupMember ? "Anoniem groepslid aanmaken" : null}
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <FormGroup>
                    <ControlLabel htmlFor="name">
                        {this.props.creatingGroup ? "Geef deze groep een naam, bijv. de naam van je huis:" : null}
                        {this.props.creatingTaskGroup ? "Geef de taakgroep een naam, bijvoorbeeld avondeten of zaterdag:" : null}
                        {this.props.creatingAnonymousGroupMember ? "Geef het nieuwe groepslid een naam:" : null}
                    </ControlLabel>
                    <FormControl autoFocus type="text" id="name" placeholder="Vul hier de nieuwe naam in" onChange={this.handleChange} onKeyPress={this.handleKeyPress} />
                </FormGroup>
            </Modal.Body>
            <Modal.Footer>
                <Button bsStyle="primary" onClick={this.handleCreateSimpleEntity}>Aanmaken</Button>
                <Button onClick={this.handleClose}>Sluiten</Button>
            </Modal.Footer>
        </Modal>;
    }
}