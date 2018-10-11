import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { QRCode } from 'react-qr-svg';
import { Api } from '../Api';
import './LinkGroupMember.css';

export class LinkGroupMember extends Component {
    constructor(props) {
        super(props);

        this.state = { invitationSecret: null };
    }

    fetch() {
        Api.getInstance().Invitation.GetSecret({ groupId: this.props.match.params.groupId }).then(result => {
            if (result.succeeded) {
                this.setState({ invitationSecret: result.invitationSecret });
            } else {
                alert("Failed!");
            }
        });
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.match.params.groupId !== this.props.match.params.groupId) {
            this.fetch();
        }
    }

    render() {
        return <div>
            <h1>Nieuw groepslid koppelen</h1>
            <p>Laat deze QR-code scannen door een iemand anders die ook over de app beschikt.</p>

            {this.state.invitationSecret ? <p>
                    U kunt ook deze link delen:<br />
                    <Link to={'/link-group/' + this.state.invitationSecret} onClick={(e) => e.preventDefault() || alert("U bent al lid van de groep!")}>Word lid van mijn <i>Wie doet de afwas?</i>-groep!</Link>
                <br /></p> : <p>QR-code aan het genereren...</p>}

            <div className="qr-code-wrapper">
                {this.state.invitationSecret ? <QRCode className="qr-code" value={this.state.invitationSecret} /> : null}
            </div>
        </div>;
    }
}