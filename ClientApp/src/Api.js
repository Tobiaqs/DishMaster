import React from 'react';

export const LS_AUTH_TOKEN = "LS_AUTH_TOKEN";

export const AuthContext = React.createContext({
    loggedIn: null,
    setAuthToken: () => {}
});

export class Api {
    constructor() {
        const apiDescription = `
            anon POST api/Auth/Login (LoginViewModel)
            anon PUT api/Auth/Register (RegisterViewModel)
            
            auth GET api/Group/List
            auth PUT api/Group/Create (CreateGroupViewModel)
            auth DELETE api/Group/Delete?groupId=#
            auth GET api/Group/Get?groupId=#
            auth GET api/Group/GetGroupMember?groupMemberId=#
            auth POST api/Group/PromoteGroupMember?groupMemberId=#
            auth POST api/Group/DemoteGroupMember?groupMemberId=#
            auth POST api/Group/ResetGroupMemberScore?groupMemberId=#
            auth POST api/Group/UpdateGroupMember
            auth GET api/Group/GetGroupRoles?groupId=#
            auth PATCH api/Group/Update (UpdateGroupViewModel)
            auth PUT api/Group/AddAnonymousGroupMember (AddAnonymousGroupMemberViewModel)
            auth DELETE api/Group/DeleteGroupMember?groupMemberId=#
            auth DELETE api/Group/LeaveGroup?groupId=#
            
            auth PUT api/Invitation/Accept?invitationSecret=#
            auth POST api/Invitation/GetSecret?groupId=#
            
            auth GET api/Task/Get?taskId=#
            auth PUT api/Task/Create (CreateTaskViewModel)
            auth DELETE api/Task/Delete?taskId=#
            auth PATCH api/Task/Update (UpdateTaskViewModel)
            
            auth GET api/TaskGroup/List?groupId=#
            auth POST api/TaskGroup/ListTaskGroupRecords
            auth GET api/TaskGroup/Get?taskGroupId=#
            auth PUT api/TaskGroup/Create (CreateTaskGroupViewModel)
            auth DELETE api/TaskGroup/Delete?taskGroupId=#
            auth PATCH api/TaskGroup/Update (UpdateTaskGroupViewModel)
            
            auth GET api/TaskGroupRecord/Get?taskGroupRecordId=#
            auth GET api/TaskGroupRecord/List?taskGroupId=#
            auth PUT api/TaskGroupRecord/Create (CreateTaskGroupRecordViewModel)
            auth PATCH api/TaskGroupRecord/AssignTask (AssignTaskViewModel)
            auth PATCH api/TaskGroupRecord/UnassignTask (UnassignTaskViewModel)
            auth DELETE api/TaskGroupRecord/Delete?taskGroupRecordId=#
            auth PATCH api/TaskGroupRecord/Finalize?taskGroupRecordId=#
        `;

        const apiEndpoints = apiDescription.trim().split("\n");

        apiEndpoints.forEach((endpoint) => {
            endpoint = endpoint.trim();
            if (endpoint.length === 0) {
                return;
            }

            let [protection, method, path, model] = endpoint.split(" ");
            
            if (model) {
                model = model.substr(1, model.length - 2);
            }

            let controller = path.split("/")[1];

            if (!this[controller]) {
                this[controller] = {};
            }

            let action = path.split("/")[2].split("?")[0];

            this[controller][action] = (data) => {
                let options = {
                    method: method,
                    headers: {}
                };

                if (protection === "auth") {
                    options.headers.Authorization = "Bearer " + localStorage.getItem(LS_AUTH_TOKEN);
                }

                const keys = data ? Object.keys(data) : [];
                if (keys.length === 1 && path.indexOf("?") !== -1) {
                    return fetch(path.replace("#", data[keys[0]]), options).then((result) => result.json());
                } else {
                    if (data) {
                        options.headers["Content-Type"] = "application/json; charset=utf-8";
                        options.body = JSON.stringify(data);
                    }
                    return fetch(path, options).then((result) => result.json());
                }
            };
        });
    }

    persistAuthToken(authToken) {
        if (authToken) {
            localStorage.setItem(LS_AUTH_TOKEN, authToken);
        } else {
            localStorage.removeItem(LS_AUTH_TOKEN);
        }
    }

    static _instance;

    static getInstance() {
        if (!Api._instance) {
            Api._instance = new Api();
        }
        return Api._instance;
    }
}