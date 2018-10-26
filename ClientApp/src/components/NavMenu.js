import React, { Component } from 'react';
import { Redirect } from 'react-router';
import { Api, AuthContext } from '../Api';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';
import { ModalCreateSimple } from './ModalCreateSimple';
import { ModalConfirm } from './ModalConfirm';

class NavMenuNoContext extends Component {
  constructor(props) {
    super(props);

    this.state = {
      groups: null,
      creatingGroup: false,
      loggingOut: false,
      loggedOut: false
    };

    this.createGroup = this.createGroup.bind(this);
    this.onModalCreateSimpleEntity = this.onModalCreateSimpleEntity.bind(this);
    this.onModalHide = this.onModalHide.bind(this);
    this.onModalConfirmed = this.onModalConfirmed.bind(this);
    this.logOut = this.logOut.bind(this);
    this.updateGroupList = this.updateGroupList.bind(this);
  }

  componentDidMount() {
    if (this.props.auth.loggedIn) {
      Api.getInstance().Group.List().then(result => {
        this.setState({ groups: result.payload });
      });
    }
  }

  updateGroupList() {
    this.setState({ groups: null });

    Api.getInstance().Group.List().then(result => {
      this.setState({ groups: result.payload });
    });
  }

  componentDidUpdate(prevProps, prevState, snapshot) {
    if ((!prevProps.auth.loggedIn && this.props.auth.loggedIn) || (!prevProps.groupList.needsUpdate && this.props.groupList.needsUpdate)) {
      this.updateGroupList();
    }

    if (this.props.groupList.needsUpdate) {
      this.props.groupList.setNeedsUpdate(false);
    }

    if (!prevState.loggedOut && this.state.loggedOut) {
      this.setState({ loggedOut: false });
    }

    if (!prevState.createdGroupId && this.state.createdGroupId) {
      this.setState({ createdGroupId: false });
    }
  }

  createGroup() {
    this.setState({ creatingGroup: true });
  }

  onModalCreateSimpleEntity(name) {
    // todo more validation?
    if (name.length > 0) {
      Api.getInstance().Group.Create({
        name: name
      }).then(result => {
        this.onModalHide();
        if (result.succeeded) {
          this.setState({ createdGroupId: result.groupId });
          this.updateGroupList();
        } else {
          alert("Failed!");
        }
      });
    }
  }

  logOut() {
    this.setState({ loggingOut: true });
  }

  onModalConfirmed() {
    this.onModalHide();
    this.setState({ loggedOut: true });
  }

  onModalHide() {
    this.setState({ creatingGroup: false, loggingOut: false });
  }

  renderGroupList(auth) {
    if (this.props.auth.loggedIn) {
      if (this.state.groups) {
        return this.state.groups.map((group) => (
          <LinkContainer to={'/group/' + group.id} exact key={group.id}>
            <NavItem>
              <Glyphicon glyph='folder-close' /> {group.name}
            </NavItem>
          </LinkContainer>
        )).concat([
          <NavItem key="new" onClick={this.createGroup}>
            <Glyphicon glyph='plus-sign' /> Groep aanmaken
          </NavItem>,
          <LinkContainer to="/link-group-qr" exact key="link">
            <NavItem>
              <Glyphicon glyph='qrcode' /> QR-code scannen
            </NavItem>
          </LinkContainer>
        ]);
      } else {
        return <NavItem>
          <Glyphicon glyph='folder-close' /> Laden...
        </NavItem>
      }
    }
  }

  renderLoginItem(auth) {
    if (!this.props.auth.loggedIn) {
      return [
        <LinkContainer to={'/login'} exact key="login">
          <NavItem>
            <Glyphicon glyph='user' /> Inloggen
          </NavItem>
        </LinkContainer>,
        <LinkContainer to={'/register'} exact key="register">
          <NavItem>
            <Glyphicon glyph='plus-sign' /> Registreren
          </NavItem>
        </LinkContainer>
      ];
    }
  }

  renderLogoutItem(auth) {
    if (this.props.auth.loggedIn) {
      return <NavItem key="logout" onClick={this.logOut}>
        <Glyphicon glyph='user' /> Uitloggen
      </NavItem>;
    }
  }

  render() {
    return <div>
      <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to={'/group-overview'}>Wie doet de afwas?</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            <LinkContainer to={'/'} exact>
              <NavItem>
                <Glyphicon glyph='home' /> Home
              </NavItem>
            </LinkContainer>
            {this.renderLoginItem()}
            {this.renderGroupList()}
            {this.renderLogoutItem()}
          </Nav>
        </Navbar.Collapse>
      </Navbar>
      <ModalCreateSimple
        creatingGroup={this.state.creatingGroup}
        onCreateSimpleEntity={this.onModalCreateSimpleEntity}
        onHide={this.onModalHide} />
      <ModalConfirm
        show={this.state.loggingOut}
        onHide={this.onModalHide}
        onConfirmed={this.onModalConfirmed}
        message="Weet je zeker dat je uit wil loggen?" />
      {this.state.loggedOut ? <Redirect key="redirect1" to="/logout" push={false} /> : null}
      {this.state.createdGroupId ? <Redirect key="redirect2" to={'/group/' + this.state.createdGroupId} push={false} /> : null}
    </div>;
  }
}

export const NavMenu = props => <AuthContext.Consumer>
  {auth => (
    <GroupListContext.Consumer>
      {groupList => <NavMenuNoContext {...props} auth={auth} groupList={groupList} />}
    </GroupListContext.Consumer>
  )}
</AuthContext.Consumer>;

export const GroupListContext = React.createContext({
  needsUpdate: null,
  setNeedsUpdate: () => {}
});