import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api, AuthContext } from '../Api';
import { Validation } from '../Validation';
import { FormGroup, FormControl, FormLabel, Button, FormText } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import './Login.css';

class LoginNoContext extends Component {
    constructor(props) {
        super(props);

        this.state = {
            loggedIn: false,
            forgotPassword: false
        };

        this.logIn = this.logIn.bind(this);
    }

    logIn(values, setSubmitting, setFieldError) {
        Api.getInstance().Auth.Login(values).then(result => {
            setSubmitting(false);
            if (result.succeeded) {
                Api.getInstance().persistAuthToken(result.token);
                this.props.auth.setAuthToken(result.token);
                this.setState({ loggedIn: true });
            } else {
                setFieldError("password", "Ongeldig wachtwoord");
            }
        });
    }

    render() {
        return <div>
            <h1>Inloggen</h1>
            <Formik
                initialValues={{ email: '', password: '' }}
                validate={values => Validation.getInstance().validateForm(values)}
                onSubmit={(values, { setSubmitting, setFieldError }) => {
                    this.logIn(values, setSubmitting, setFieldError);
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
                            <FormLabel htmlFor="password">Wachtwoord:</FormLabel>
                            <Field type="password" name="password" className="form-control" />
                            <FormControl.Feedback />
                            <ErrorMessage name="password" component={FormText} />
                        </FormGroup>
                        <FormGroup>
                            <Button type="submit" disabled={isSubmitting}>
                                Inloggen
                            </Button>
                            &nbsp;
                            <Button onClick={() => this.setState({ forgotPassword: true })}>
                                Wachtwoord vergeten?
                            </Button>
                        </FormGroup>
                    </Form>
                )}
            </Formik>
            {this.state.loggedIn ? <Redirect to="/" push={false} key="redirect" /> : null}
            {this.state.forgotPassword ? <Redirect to="/login/forgot" push={false} key="redirect" /> : null}
        </div>;
    }
}

export const Login = props => <AuthContext.Consumer>
    {auth => <LoginNoContext {...props} auth={auth} />}
</AuthContext.Consumer>