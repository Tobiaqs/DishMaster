import React, { Component } from 'react';
import { Api } from '../Api';
import { Redirect } from 'react-router';
import { GroupListContext } from './NavMenu';

class LinkGroupNoContext extends Component {
    constructor(props) {
        super(props);

        this.state = {
            newGroupId: null,
            errorOccurred: false
        };
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.invitationSecret !== this.props.match.params.invitationSecret) {
            this.setState({ errorOccurred: false });
            this.fetch();
        }
    }

    fetch() {
        const invitationSecret = this.props.match.params.invitationSecret;
        if (invitationSecret && invitationSecret.length === 36) {
            Api.getInstance().Invitation.Accept({ invitationSecret: invitationSecret }).then(result => {
                if (result.succeeded) {
                    this.props.groupList.setNeedsUpdate(true);
                    this.setState({ newGroupId: result.groupId });
                } else {
                    this.setState({ errorOccurred: true });
                }
            });
        } else {
            this.setState({ errorOccurred: true });
        }
    }

    render() {
        return <div>
            {this.state.errorOccurred ? <h1>Uitnodiging accepteren mislukt</h1> : <h1>Uitnodiging accepteren&#8230;</h1>}
            {this.state.errorOccurred ? <p>Deze uitnodiging is niet geldig.</p> : null}
            {this.state.newGroupId ? <Redirect to={'/group/' + this.state.newGroupId} push={false} /> : null}
        </div>;
    }
}

export const LinkGroup = props => <GroupListContext.Consumer>
    {groupList => <LinkGroupNoContext {...props} groupList={groupList} />}
</GroupListContext.Consumer>;