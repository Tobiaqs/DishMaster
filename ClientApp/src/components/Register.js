import React, { Component } from 'react';
import { Redirect } from 'react-router';
import { FormGroup, FormControl, FormLabel, Button, FormText } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Api, AuthContext } from '../Api';
import { Validation } from '../Validation';
import { GroupListContext } from './NavMenu';

class RegisterNoContext extends Component {
    constructor(props) {
        super(props);

        this.state = {
            registered: false
        }
    }

    register = (values, setSubmitting, setFieldError) => {
        Api.getInstance().Auth.Register(values).then((result) => {
            setSubmitting(false);
            if (result.succeeded) {
                this.props.auth.setAuthToken(result.token);
                this.props.groupList.setNeedsUpdate(true);
                this.setState({ registered: true });
            } else {
                setFieldError("email", "Dit e-mailadres bestaat al");
            }
        });
    }

    render() {
        return <div className="form-wrapper">
            <h1>Registeren</h1>
            <Formik
                initialValues={{ fullName: '', email: '', password: '', passwordAgain: '' }}
                validate={values => Validation.getInstance().validateForm(values)}
                onSubmit={(values, { setSubmitting, setFieldError }) => this.register(values, setSubmitting, setFieldError)}>
                {({ isSubmitting, errors, values }) => (
                    <Form className="form-wrapper">
                        <FormGroup>
                            <FormLabel htmlFor="fullName">Naam:</FormLabel>
                            <Field type="text" name="fullName" className="form-control" />
                            <FormControl.Feedback />
                            <ErrorMessage name="fullName" component={FormText} />
                        </FormGroup>
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
                            <FormLabel htmlFor="passwordAgain">Wachtwoord (nogmaals):</FormLabel>
                            <Field type="password" name="passwordAgain" className="form-control" />
                            <FormControl.Feedback />
                            <ErrorMessage name="passwordAgain" component={FormText} />
                        </FormGroup>
                        <FormGroup>
                            <Button type="submit" disabled={isSubmitting}>
                                Registreren
                            </Button>
                        </FormGroup>
                    </Form>
                )}
            </Formik>
            {this.state.registered ? <Redirect to="/" push={false} key="redirect" /> : null}
        </div>;
    }
}

export const Register = props => <AuthContext.Consumer>
    {auth => <GroupListContext.Consumer>
        {groupList => <RegisterNoContext {...props} auth={auth} groupList={groupList} />}
    </GroupListContext.Consumer>}
</AuthContext.Consumer>;