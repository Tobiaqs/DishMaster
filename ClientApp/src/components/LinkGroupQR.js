import React, { Component } from 'react';
import QrReader from 'react-qr-reader';
import { Redirect } from 'react-router';

export class LinkGroupQR extends Component {
    constructor(props) {
        super(props);

        this.state = {
            invitationSecret: null,
            errorOccurred: false,
            invalidData: false
        };

        this.handleScan = this.handleScan.bind(this);
        this.handleError = this.handleError.bind(this);
    }

    handleScan(data, groupList) {
        if (data) {
            if (data.length === 36) {
                this.setState({ invitationSecret: data, invalidData: false });
            } else {
                this.setState({ invalidData: true });
            }
        }
    }

    handleError(err) {
        this.setState({ errorOccurred: true });
    }

    render() {
        return <div>
            <h1>QR-code scannen</h1>
            <p>U kunt een QR-code van andermans scherm scannen om lid te worden van een groep.</p>
            {!this.state.invitationSecret && !this.state.errorOccurred ? <QrReader
                delay={5000}
                onError={this.handleError}
                onScan={this.handleScan}
                style={{ width: "100%" }}
                /> : null}
            {this.state.errorOccurred ? <p>Gelieve camerapermissie te verlenen.</p> : null}
            {this.state.invalidData ? <p>Deze QR-code is niet te gebruiken.</p> : null}
            {this.state.invitationSecret ? <Redirect to={'/link-group/' + this.state.invitationSecret} push={false} /> : null}
        </div>;
    }
}