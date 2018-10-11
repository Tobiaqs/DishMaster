import React, { Component } from 'react';

import { FormGroup, FormControl, ControlLabel, Modal, Button } from 'react-bootstrap';

export class ModalEditSimple extends Component {
    constructor(props) {
        super(props);

        this.state = {
            name: props.originalName || ''
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleKeyPress = this.handleKeyPress.bind(this);
        this.handleRenameSimpleEntity = this.handleRenameSimpleEntity.bind(this);
        this.handleDeleteSimpleEntity = this.handleDeleteSimpleEntity.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    // reset the state when the modal is closed
    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.originalName !== this.props.originalName) {
            this.setState({ name: this.props.originalName || '' });
        }
    }

    handleChange(event) {
        const update = {};
        update[event.target.id] = event.target.value;
        this.setState(update);
    }

    handleKeyPress(event) {
        if (event.key === 'Enter') {
            this.handleRenameSimpleEntity(this.state.name);
        }
    }

    handleRenameSimpleEntity() {
        this.props.onRenameSimpleEntity(this.state.name);
    }

    handleDeleteSimpleEntity() {
        this.props.onDeleteSimpleEntity();
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.editingGroup || this.props.editingTaskGroup} onHide={this.handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>
                    {this.props.editingGroup ? "Groep hernoemen of verwijderen" : null}
                    {this.props.editingTaskGroup ? "Takengroep hernoemen of verwijderen" : null}
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <FormGroup>
                    <ControlLabel htmlFor="name">
                        {this.props.editingGroup ? "Geef deze groep een andere naam:" : null}
                        {this.props.editingTaskGroup ? "Geef deze takengroep een andere naam:" : null}
                    </ControlLabel>
                    <FormControl autoFocus value={this.state.name} type="text" id="name" placeholder="Vul hier de nieuwe naam in" onChange={this.handleChange} onKeyPress={this.handleKeyPress} />
                </FormGroup>
            </Modal.Body>
            <Modal.Footer>
                <Button bsStyle="primary" onClick={this.handleRenameSimpleEntity}>Hernoemen</Button>
                <Button bsStyle="danger" onClick={this.handleDeleteSimpleEntity}>Verwijderen</Button>
                <Button onClick={this.handleClose}>Sluiten</Button>
            </Modal.Footer>
        </Modal>;
    }
}