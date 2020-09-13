import React, { Component } from 'react';
import { Redirect } from 'react-router';
import { Api, AuthContext } from '../Api';
import { Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';
import { ModalCreateSimple } from './ModalCreateSimple';
import { ModalConfirm } from './ModalConfirm';
import { FaTimes, FaPlus, FaUsers, FaQrcode, FaUser, FaHome, FaBook } from 'react-icons/fa';

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

  renderGroupList() {
    if (this.props.auth.loggedIn) {
      if (this.state.groups) {
        let groupList = null;

        if (this.state.groups.length === 1) {
          const group = this.state.groups[0];
          groupList = <LinkContainer to={'/group/' + group.id} replace={true} exact key={group.id}>
            <Nav.Link>
              <FaUsers /> {group.name}
            </Nav.Link>
          </LinkContainer>;
        } else if (this.state.groups.length > 1) {
          groupList = <NavDropdown key="groups" title={<span><FaUsers /> Groepen</span>} id="group-dropdown">{this.state.groups.map(group => (
            <LinkContainer to={'/group/' + group.id} replace={true} exact key={group.id}>
              <NavDropdown.Item>
                {group.name}
              </NavDropdown.Item>
            </LinkContainer>
          ))}</NavDropdown>;
        }
        return [groupList,
          <Nav.Link key="new" onClick={this.createGroup}>
            <FaPlus /> Groep aanmaken
          </Nav.Link>,
          <LinkContainer to="/link-group-qr" replace={true} exact key="link">
            <Nav.Link>
              <FaQrcode /> QR-code scannen
            </Nav.Link>
          </LinkContainer>
        ];
      } else {
        return <Nav.Link>
          <FaTimes /> Laden...
        </Nav.Link>
      }
    }
  }

  renderLoginItem() {
    if (!this.props.auth.loggedIn) {
      return [
        <LinkContainer to={'/login'} replace={true} exact key="login">
          <Nav.Link>
            <FaUser /> Inloggen
          </Nav.Link>
        </LinkContainer>,
        <LinkContainer to={'/register'} replace={true} exact key="register">
          <Nav.Link>
            <FaPlus /> Registreren
          </Nav.Link>
        </LinkContainer>
      ];
    }
  }

  renderLogoutItem() {
    if (this.props.auth.loggedIn) {
      return <Nav.Link key="logout" onClick={this.logOut}>
        <FaUser /> Uitloggen
      </Nav.Link>;
    }
  }

  render() {
    return <div>
        <Navbar fixed="top" bg="light" expand="lg">
          <LinkContainer to={'/'} replace={true} exact>
            <Navbar.Brand>WieDoetDeAfwas</Navbar.Brand>
          </LinkContainer>
          <Navbar.Toggle aria-controls="main-nav" />
          <Navbar.Collapse id="main-nav">
            <Nav className="mr-auto">
              <LinkContainer to={'/'} replace={true} exact>
                <Nav.Link>
                  <FaHome /> Home
                </Nav.Link>
              </LinkContainer>
              {this.renderLoginItem()}
              {this.renderGroupList()}
              <LinkContainer to={'/manual'} replace={true} exact>
                <Nav.Link>
                  <FaBook /> Handleiding
                </Nav.Link>
              </LinkContainer>
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