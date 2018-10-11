import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api, AuthContext } from '../Api';
import { FormGroup, FormControl, ControlLabel, Button } from 'react-bootstrap';
import './Login.css';

class LoginNoContext extends Component {
    constructor(props) {
        super(props);

        this.state = {
            email: '',
            password: '',
            loggedIn: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleKeyPress = this.handleKeyPress.bind(this);
        this.logIn = this.logIn.bind(this);
    }

    handleChange(event) {
        const update = {};
        update[event.target.id] = event.target.value;
        this.setState(update);
    }

    handleKeyPress(event) {
        if (event.key === 'Enter') {
            this.logIn();
        }
    }

    logIn() {
        Api.getInstance().Auth.Login(this.state).then((result) => {
            if (result.succeeded) {
                Api.getInstance().persistAuthToken(result.token);
                this.props.auth.setAuthToken(result.token);
                this.setState({ loggedIn: true });
            }
        });
    }

    render() {
        return [
            <AuthContext.Consumer key="main">
                {auth => (
                    <div className="form-wrapper">
                        <h1>Inloggen</h1>
                        <FormGroup>
                            <ControlLabel htmlFor="email">E-mailadres:</ControlLabel>
                            <FormControl type="email" id="email" onChange={this.handleChange} onKeyPress={this.handleKeyPress} />
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel htmlFor="password">Wachtwoord:</ControlLabel>
                            <FormControl type="password" id="password" onChange={this.handleChange} onKeyPress={this.handleKeyPress} />
                        </FormGroup>
                        <Button bsStyle="primary" block onClick={this.logIn}>Inloggen</Button>
                    </div>
                )}
            </AuthContext.Consumer>,
            this.state.loggedIn ? <Redirect to="/" push={false} key="redirect" /> : null
        ];
    }
}

export const Login = props => <AuthContext.Consumer>
    {auth => <LoginNoContext {...props} auth={auth} />}
</AuthContext.Consumer>