import React, { Component } from 'react';

import { Table } from 'react-bootstrap';
import { Tools } from './Tools';

export class TaskGroupRecordPresence extends Component {
    constructor(props) {
        super(props);

    }

    render() {
        return <div>
            <h4>Aanwezigen</h4>
            <Table striped bordered condensed hover>
                <thead>
                    <tr>
                        <th>Groepslid</th>
                        <th>Aanwezig</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.taskGroupRecord.presentGroupMembersIds.map(groupMemberId => {
                        return { name: Tools.getGroupMemberName(this.props.group, groupMemberId), present: true };
                    }).concat(this.props.group.groupMembers.map(groupMember => {
                        if (this.props.taskGroupRecord.presentGroupMembersIds.indexOf(groupMember.id) === -1) {
                            return { name: Tools.getGroupMemberName(this.props.group, groupMember.id), present: false };
                        }
                        return null;
                    }))
                    .filter(groupMemberDescriptor => groupMemberDescriptor !== null)
                    .sort((a, b) => {
                        const aLower = a.name.toLowerCase();
                        const bLower = b.name.toLowerCase();
                        if (aLower === bLower) {
                            return 0;
                        }
                        return aLower > bLower ? 1 : -1;
                    })
                    .map(groupMemberDescriptor => {
                        if (groupMemberDescriptor !== null) {
                            return <tr><td>{groupMemberDescriptor.name}</td><td>{groupMemberDescriptor.present ? "Ja" : "Nee"}</td></tr>;
                        }
                    })}
                </tbody>
            </Table>
        </div>;
    }
}