import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api } from '../Api';
import { Validation } from '../Validation';
import { ModalNotification } from './ModalNotification';
import { FormGroup, FormControl, FormLabel, Button, FormText } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import './Login.css';

export class ForgotPassword extends Component {
    constructor(props) {
        super(props);

        this.state = {
            done: false,
            notificationVisible: false
        };
    }

    request = (values, setSubmitting, setFieldError) => {
        Api.getInstance().Auth.ForgotPassword(values).then(result => {
            setSubmitting(false);
            if (result.succeeded) {
                this.setState({ notificationVisible: true });
            } else if (result.httpError === 404) {
                setFieldError("email", "E-mailadres niet gevonden");
            } else {
                alert("SMTP not available");
            }
        });
    }

    forgot = () => {
        this.setState({ notificationVisible: true });
    }

    onModalHide = () => {
        this.setState({ notificationVisible: false, done: true });
    }

    render() {
        return <div>
            <h1>Wachtwoord vergeten</h1>
            <Formik
                initialValues={{ email: '' }}
                validate={values => Validation.getInstance().validateForm(values)}
                onSubmit={(values, { setSubmitting, setFieldError }) => {
                    this.request(values, setSubmitting, setFieldError);
                }}>
                {({ isSubmitting, errors, values }) => (
                    <Form className="form-wrapper">
                        <FormGroup>
                            <FormLabel htmlFor="email">E-mailadres:</FormLabel>
                            <Field type="email" name="email" className="form-control" />
                            <FormControl.Feedback />
                            <ErrorMessage name="email" component={FormText} />
                        </FormGroup>
                        <FormGroup>
                            <Button type="submit" disabled={isSubmitting}>
                                Wachtwoord opvragen
                            </Button>
                        </FormGroup>
                    </Form>
                )}
            </Formik>

            {this.state.done ? <Redirect to="/" push={false} key="redirect" /> : null}
            <ModalNotification
                show={this.state.notificationVisible}
                title="Gelukt"
                message="Er is een e-mail verzonden naar dit adres met daarin instructies hoe u uw wachtwoord opnieuw kunt instellen."
                onHide={this.onModalHide} />
        </div>;
    }
}
