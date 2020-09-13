import React, { Component } from 'react';
import { FormGroup, FormControl, FormLabel, Modal, Button, FormText } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Validation } from '../Validation';

export class ModalCreateSimple extends Component {
    constructor(props) {
        super(props);

        this.handleCreateSimpleEntity = this.handleCreateSimpleEntity.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleCreateSimpleEntity(name) {
        this.props.onCreateSimpleEntity(name);
    }

    handleClose() {
        this.props.onHide();
    }

    render() {
        return <Modal show={this.props.creatingGroup || this.props.creatingTaskGroup || this.props.creatingAnonymousGroupMember} onHide={this.handleClose}>
            <Formik
                initialValues={{ name: '' }}
                validate={values => Validation.getInstance().validateForm(values)}
                onSubmit={values => this.handleCreateSimpleEntity(values.name)}
                >
                {({ values, errors, isSubmitting }) => (
                    <Form>
                        <Modal.Header closeButton>
                            <Modal.Title>
                                {this.props.creatingGroup ? "Groep aanmaken" : null}
                                {this.props.creatingTaskGroup ? "Taakgroep aanmaken" : null}
                                {this.props.creatingAnonymousGroupMember ? "Anoniem groepslid aanmaken" : null}
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <FormGroup>
                                <FormLabel htmlFor="name">
                                    {this.props.creatingGroup ? "Geef deze groep een naam, bijv. de naam van je huis:" : null}
                                    {this.props.creatingTaskGroup ? "Geef de taakgroep een naam, bijvoorbeeld avondeten of zaterdag:" : null}
                                    {this.props.creatingAnonymousGroupMember ? "Geef het nieuwe groepslid een naam:" : null}
                                </FormLabel>
                                <Field autoFocus type="text" name="name" autoComplete="off" className="form-control" placeholder="Vul hier de nieuwe naam in" />
                                <FormControl.Feedback />
                                <ErrorMessage name="name" component={FormText} />
                            </FormGroup>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="primary" type="submit" disabled={isSubmitting}>Aanmaken</Button>
                        </Modal.Footer>
                    </Form>
                )}
            </Formik>
        </Modal>;
    }
}