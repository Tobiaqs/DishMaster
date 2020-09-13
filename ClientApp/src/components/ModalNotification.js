import React, { Component } from 'react';

import { Modal, Button } from 'react-bootstrap';

export class ModalNotification extends Component {
    constructor(props) {
        super(props);

        this.handleClose = this.handleClose.bind(this);
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.show} onHide={this.handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>
                    {this.props.title}
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {this.props.message}
            </Modal.Body>
            <Modal.Footer>
                <Button variant="primary" onClick={this.handleClose}>Sluiten</Button>
            </Modal.Footer>
        </Modal>;
    }
}