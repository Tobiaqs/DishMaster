import { Redirect } from 'react-router';
import React, { Component } from 'react';
import { Api, AuthContext } from '../Api';
import { ListGroup, ListGroupItem } from 'react-bootstrap';

export class GroupOverview extends Component {
    constructor(props) {
        super(props);

        this.state = { groups: null };

        this.goToGroup = this.goToGroup.bind(this);
    }

    componentDidMount() {
        this.fetch();
    }

    fetch() {
        Api.getInstance().Group.List().then(result => {
            this.setState({ groups: result.payload });
        });
    }

    goToGroup(group) {
        this.setState({ selectedGroupId: group.id });
    }

    render() {
        return <AuthContext.Consumer>
            {auth => <div>
                {this.state.groups ? <h1>Groepenoverzicht</h1> : <h1>Laden&#8230;</h1>}
                <ListGroup>
                    {this.state.groups ? this.state.groups.map(group => <ListGroupItem action
                        key={group.id}
                        onClick={() => this.goToGroup(group)}>
                        {group.name}
                    </ListGroupItem>) : null}
                    {this.state.groups && this.state.groups.length === 0 ? <ListGroupItem action disabled key="none">
                        Er zijn nog geen groepen.
                    </ListGroupItem> : null}
                </ListGroup>
                {this.state.selectedGroupId ? <Redirect to={'/group/' + this.state.selectedGroupId} push={false} /> : null}
                {this.state.groups && this.state.groups.length === 1 ? <Redirect to={'/group/' + this.state.groups[0].id} push={false} /> : null}
                {!auth.loggedIn ? <Redirect to={'/login'} push={false} /> : null}
            </div>}
        </AuthContext.Consumer>;
    }
}