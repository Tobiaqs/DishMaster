import React, { Component } from 'react';

import { FormGroup, FormControl, ControlLabel, Modal, Button, HelpBlock } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Validation } from '../Validation';
import { ModalConfirm } from './ModalConfirm';

export class ModalEditSimple extends Component {
    constructor(props) {
        super(props);

        this.state = {
            deleting: false
        };

        this.handleRenameSimpleEntity = this.handleRenameSimpleEntity.bind(this);
        this.handleDeleteSimpleEntity = this.handleDeleteSimpleEntity.bind(this);
        this.showDeleteModal = this.showDeleteModal.bind(this);
        this.hideDeleteModal = this.hideDeleteModal.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleRenameSimpleEntity(name) {
        this.props.onRenameSimpleEntity(name);
    }

    handleDeleteSimpleEntity(name) {
        this.props.onDeleteSimpleEntity();
    }

    showDeleteModal() {
        this.setState({ deleting: true });
    }

    hideDeleteModal() {
        this.setState({ deleting: false });
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.editingGroup || this.props.editingTaskGroup} onHide={this.handleClose}>
            <Formik
                initialValues={{ name: this.props.originalName }}
                validate={(values) => Validation.getInstance().validateForm(values)}
                onSubmit={values => this.handleRenameSimpleEntity(values.name)}>
                {({ isSubmitting, errors, values }) => (
                    <Form>
                        <Modal.Header closeButton>
                            <Modal.Title>
                                {this.props.editingGroup ? "Groep hernoemen of verwijderen" : null}
                                {this.props.editingTaskGroup ? "Taakgroep hernoemen of verwijderen" : null}
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <FormGroup validationState={values.name ? (errors.name ? "error" : "success") : null}>
                                <ControlLabel htmlFor="name">
                                    {this.props.editingGroup ? "Geef deze groep een andere naam:" : null}
                                    {this.props.editingTaskGroup ? "Geef deze taakgroep een andere naam:" : null}
                                </ControlLabel>
                                <Field autoFocus type="text" name="name" id="name" className="form-control" placeholder="Vul hier de nieuwe naam in" />
                                <FormControl.Feedback />
                                <ErrorMessage name="name" component={HelpBlock} />
                            </FormGroup>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button bsStyle="primary" type="submit" disabled={isSubmitting}>Hernoemen</Button>
                            <Button bsStyle="danger" onClick={this.showDeleteModal}>Verwijderen</Button>
                        </Modal.Footer>
                    </Form>
                )}
            </Formik>
            <ModalConfirm
                show={this.state.deleting}
                message="Weet je zeker dat je dit wilt verwijderen?"
                onHide={this.hideDeleteModal}
                onConfirmed={this.handleDeleteSimpleEntity} />
        </Modal>;
    }
}