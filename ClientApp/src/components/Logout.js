import React, { Component } from 'react';
import { AuthContext } from '../Api';
import { Redirect } from 'react-router';

class LogoutNoContext extends Component {
    constructor(props) {
        super(props);

        this.state = { loggedOut: false };
    }

    componentDidMount() {
        this.props.auth.setAuthToken(null);
        this.setState({ loggedOut: true });
    }

    render() {
        return this.state.loggedOut ? <Redirect to="/" push={false} /> : null;
    }
}

export const Logout = props => <AuthContext.Consumer>
    {auth => <LogoutNoContext {...props} auth={auth} />}
</AuthContext.Consumer>