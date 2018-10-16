import { LS_AUTH_TOKEN, Api, AuthContext } from './Api';
import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { GroupListContext } from './components/NavMenu';
import { Home } from './components/Home';
import { Login } from './components/Login';
import { LinkGroup } from './components/LinkGroup';
import { LinkGroupQR } from './components/LinkGroupQR';
import { Logout } from './components/Logout';
import { Group } from './components/Group';
import { LinkGroupMember } from './components/LinkGroupMember';
import { GroupMember } from './components/GroupMember';
import { TaskGroup } from './components/TaskGroup';
import { TaskGroupRecord } from './components/TaskGroupRecord';
import { CreateTaskGroupRecord } from './components/CreateTaskGroupRecord';
import { Task } from './components/Task';
import { Register } from './components/Register';

export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      auth: {
        loggedIn: !!localStorage.getItem(LS_AUTH_TOKEN),
        setAuthToken: this.setAuthToken.bind(this)
      },
      
      groupList: {
        needsUpdate: false,
        setNeedsUpdate: this.setNeedsUpdate.bind(this)
      }
    };
  }

  setNeedsUpdate(value) {
    this.setState({
      auth: {
        ...this.state.auth
      },
      groupList: {
        ...this.state.groupList,
        needsUpdate: value
      }
    });
  }

  setAuthToken(authToken) {
    // Store token for use by Api
    Api.getInstance().persistAuthToken(authToken);

    this.setState({
      auth: {
        ...this.state.auth,
        loggedIn: !!authToken
      },
      groupList: {
        ...this.state.groupList
      }
    });
  }

  render() {
    return <GroupListContext.Provider value={this.state.groupList}>
      <AuthContext.Provider value={this.state.auth}>
        <Layout>
          <Route exact path='/' component={Home} />
          <Route exact path='/login' component={Login} />
          <Route exact path='/logout' component={Logout} />
          <Route exact path='/register' component={Register} />
          <Route exact path='/link-group-qr' component={LinkGroupQR} />
          <Route exact path='/link-group/:invitationSecret' component={LinkGroup} />
          <Route exact path='/group/:groupId' component={Group} />
          <Route exact path='/group/:groupId/link-group-member' component={LinkGroupMember} />
          <Route exact path='/group/:groupId/taskGroup/:taskGroupId' component={TaskGroup} />
          <Route exact path='/group/:groupId/taskGroup/:taskGroupId/create-task-group-record' component={CreateTaskGroupRecord} />
          <Route exact path='/group/:groupId/taskGroup/:taskGroupId/task/:taskId' component={Task} />
          <Route exact path='/group/:groupId/taskGroup/:taskGroupId/taskGroupRecord/:taskGroupRecordId' component={TaskGroupRecord} />
          <Route exact path='/group/:groupId/groupMember/:groupMemberId' component={GroupMember} />
        </Layout>
      </AuthContext.Provider>
    </GroupListContext.Provider>;
  }
}
