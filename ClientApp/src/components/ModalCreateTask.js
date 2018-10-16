import React, { Component } from 'react';

import { FormGroup, FormControl, ControlLabel, Modal, Button, HelpBlock } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Validation } from '../Validation';

export class ModalCreateTask extends Component {
    constructor(props) {
        super(props);

        this.handleCreateTask = this.handleCreateTask.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleCreateTask(values) {
        this.props.onCreateTask(values);
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.creatingTask} onHide={this.handleClose}>
            <Formik
                initialValues={{ name: '', bounty: '', isNeutral: false }}
                validate={values => Validation.getInstance().validateForm(values)}
                onSubmit={values => this.handleCreateTask(values)}
                >
                {({ isSubmitting, values, errors }) => (
                    <Form>
                        <Modal.Header closeButton>
                            <Modal.Title>Taak aanmaken</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <FormGroup validationState={values.name ? (errors.name ? "error" : "success") : null}>
                                <ControlLabel htmlFor="name">Geef deze taak een naam, bijv. afwassen:</ControlLabel>
                                <Field autoFocus type="text" name="name" id="name" className="form-control" placeholder="Vul hier de naam van de taak in" />
                                <FormControl.Feedback />
                                <ErrorMessage name="name" component={HelpBlock} />
                            </FormGroup>
                            <FormGroup>
                                <Field type="checkbox" name="isNeutral" id="isNeutral" /> <ControlLabel htmlFor="isNeutral">Is deze taak score-neutraal?</ControlLabel>
                                <ErrorMessage name="isNeutral" component={HelpBlock} />
                            </FormGroup>
                            {values.isNeutral ? null :
                                <FormGroup validationState={typeof(values.bounty) === "number" ? (errors.bounty ? "error" : "success") : null}>
                                    <ControlLabel htmlFor="bounty">Wat is de opbrengst van deze taak in punten? Bereik: [0, 10]</ControlLabel>
                                    <Field type="number" name="bounty" id="bounty" className="form-control" placeholder="Vul hier de opbrengst van deze taak in" />
                                    <FormControl.Feedback />
                                    <ErrorMessage name="bounty" component={HelpBlock} />
                                </FormGroup>
                            }
                        </Modal.Body>
                        <Modal.Footer>
                            <Button bsStyle="primary" type="submit" disabled={isSubmitting}>Aanmaken</Button>
                            <Button onClick={this.handleClose}>Sluiten</Button>
                        </Modal.Footer>
                    </Form>
                )}
            </Formik>
        </Modal>;
    }
}