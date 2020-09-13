import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api, AuthContext } from '../Api';
import { Validation } from '../Validation';
import { ModalNotification } from './ModalNotification';
import { FormGroup, FormControl, FormLabel, Button, FormText } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import './Login.css';

class ResetPasswordNoContext extends Component {
    constructor(props) {
        super(props);

        this.state = {
            loggedIn: false,
            errorNotificationVisible: false,
            successNotificationVisible: false
        };
    }

    resetPassword = (values, setSubmitting, setFieldError) => {
        const email = new URLSearchParams(this.props.location.search).get("email");
        const token = new URLSearchParams(this.props.location.search).get("token");
        Api.getInstance().Auth.ResetPassword({
            email: email,
            password: values.password,
            resetToken: token
         }).then(result => {
            if (result.succeeded) {
                Api.getInstance().Auth.Login({ email: email, password: values.password }).then(result => {
                    Api.getInstance().persistAuthToken(result.token);
                    this.props.auth.setAuthToken(result.token);
                    this.setState({ successNotificationVisible: true });
                    setSubmitting(false);
                }).catch(() => {
                    this.setState({ errorNotificationVisible: true });
                    setSubmitting(false);
                });
            } else {
                this.setState({ errorNotificationVisible: true });
                setSubmitting(false);
            }
        }).catch(() => {
            setSubmitting(false);
            this.setState({ errorNotificationVisible: true });
        })
    }

    onErrorModalHide = () => {
        this.setState({ errorNotificationVisible: false });
    }

    onSuccessModalHide = () => {
        this.setState({ successNotificationVisible: false, loggedIn: true });
    }

    render() {
        return <div>
            <h1>Wachtwoord herstellen</h1>
            <Formik
                initialValues={{ password: '', passwordAgain: '' }}
                validate={values => Validation.getInstance().validateForm(values)}
                onSubmit={(values, { setSubmitting, setFieldError }) => {
                    this.resetPassword(values, setSubmitting, setFieldError);
                }}>
                {({ isSubmitting, errors, values }) => (
                    <Form className="form-wrapper">
                        <FormGroup>
                            <FormLabel htmlFor="password">Wachtwoord:</FormLabel>
                            <Field type="password" name="password" className="form-control" />
                            <FormControl.Feedback />
                            <ErrorMessage name="password" component={FormText} />
                        </FormGroup>
                        <FormGroup>
                            <FormLabel htmlFor="passwordAgain">Wachtwoord (nogmaals):</FormLabel>
                            <Field type="password" name="passwordAgain" className="form-control" />
                            <FormControl.Feedback />
                            <ErrorMessage name="passwordAgain" component={FormText} />
                        </FormGroup>
                        <FormGroup>
                            <Button type="submit" disabled={isSubmitting}>
                                Wachtwoord herstellen
                            </Button>
                            &nbsp;
                        </FormGroup>
                    </Form>
                )}
            </Formik>
            {this.state.loggedIn ? <Redirect to="/" push={false} key="redirect" /> : null}
            <ModalNotification
                show={this.state.errorNotificationVisible}
                title="Fout"
                message="Het token is niet langer geldig, of dit e-mailadres bestaat niet."
                onHide={this.onErrorModalHide} />
            <ModalNotification
                show={this.state.successNotificationVisible}
                title="Gelukt"
                message="Uw wachtwoord is hersteld. U bent meteen ingelogd."
                onHide={this.onSuccessModalHide} />
        </div>;
    }
}

export const ResetPassword = props => <AuthContext.Consumer>
    {auth => <ResetPasswordNoContext {...props} auth={auth} />}
</AuthContext.Consumer>