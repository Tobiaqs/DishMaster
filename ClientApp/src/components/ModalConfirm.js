import React, { Component } from 'react';

import { Modal, Button } from 'react-bootstrap';

export class ModalConfirm extends Component {
    constructor(props) {
        super(props);

        this.handleConfirmed = this.handleConfirmed.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleConfirmed() {
        this.props.onConfirmed();
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.show} onHide={this.handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Zeker weten?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {this.props.message}
            </Modal.Body>
            <Modal.Footer>
                <Button bsStyle="primary" onClick={this.handleConfirmed}>Zeker</Button>
            </Modal.Footer>
        </Modal>;
    }
}