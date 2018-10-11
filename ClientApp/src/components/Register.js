import React, { Component } from 'react';
import { Redirect } from 'react-router';
import { FormGroup, FormControl, ControlLabel, Button } from 'react-bootstrap';
import { Api, AuthContext } from '../Api';
import { GroupListContext } from './NavMenu';

export class Register extends Component {
    constructor(props) {
        super(props);

        this.state = {
            fullName: '',
            email: '',
            password: '',
            passwordAgain: '',
            registered: false
        }

        this.register = this.register.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleKeyPress = this.handleKeyPress.bind(this);
    }

    register(auth, groupList) {
        if (this.state.password !== this.state.passwordAgain) {
            return alert('Your passwords do not match.');
        }
        Api.getInstance().Auth.Register(this.state).then((result) => {
            if (result.succeeded) {
                alert('Succeeded!');
                console.log(auth);

                auth.setAuthToken(result.token);
                groupList.setNeedsUpdate(true);
                this.setState({ registered: true });
            } else {
                console.log(result);
                alert('Failed!');
            }
        });
    }

    handleChange(event) {
        const update = {};
        update[event.target.id] = event.target.value;
        this.setState(update);
    }

    handleKeyPress(event, auth, groupList) {
        if (event.key === 'Enter') {
            this.register(auth, groupList);
        }
    }

    render() {
        return [
            <AuthContext.Consumer key="main">
                {auth => <GroupListContext.Consumer>
                    {groupList => <div className="form-wrapper">
                        <h1>Registeren</h1>
                        <FormGroup>
                            <ControlLabel htmlFor="fullName">Naam:</ControlLabel>
                            <FormControl type="text" id="fullName" onChange={this.handleChange} onKeyPress={(event) => this.handleKeyPress(event, auth, groupList)} />
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel htmlFor="email">E-mailadres:</ControlLabel>
                            <FormControl type="email" id="email" onChange={this.handleChange} onKeyPress={(event) => this.handleKeyPress(event, auth, groupList)} />
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel htmlFor="password">Wachtwoord:</ControlLabel>
                            <FormControl type="password" id="password" onChange={this.handleChange} onKeyPress={(event) => this.handleKeyPress(event, auth, groupList)} />
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel htmlFor="passwordAgain">Wachtwoord (nogmaals):</ControlLabel>
                            <FormControl type="password" id="passwordAgain" onChange={this.handleChange} onKeyPress={(event) => this.handleKeyPress(event, auth, groupList)} />
                        </FormGroup>
                        <Button bsStyle="primary" block onClick={() => this.register(auth, groupList)}>Registreren</Button>
                    </div>}
                </GroupListContext.Consumer>}
            </AuthContext.Consumer>,
            this.state.registered ? <Redirect to="/" push={false} key="redirect" /> : null
        ];
    }
}