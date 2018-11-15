import React, { Component } from 'react';

import { Table } from 'react-bootstrap';
import { Tools } from './Tools';

export class TaskGroupRecordPresence extends Component {
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
                        return {
                            name: groupMemberId ? Tools.getGroupMemberName(this.props.group, groupMemberId) : null,
                            present: true,
                            id: groupMemberId
                        };
                    }).concat(this.props.group.groupMembers.map(groupMember => {
                        if (this.props.taskGroupRecord.presentGroupMembersIds.indexOf(groupMember.id) === -1) {
                            return {
                                name: Tools.getGroupMemberName(this.props.group, groupMember.id),
                                present: false,
                                id: groupMember.id
                            };
                        }
                        return null;
                    }))
                    .filter(groupMemberDescriptor => groupMemberDescriptor !== null)
                    .sort((a, b) => {
                        if (a.name === null && b.name === null) {
                            return 0;
                        }
                        if (a.name === null) {
                            return 1;
                        }
                        if (b.name === null) {
                            return -1;
                        }
                        const aLower = a.name.toLowerCase();
                        const bLower = b.name.toLowerCase();
                        if (aLower === bLower) {
                            return 0;
                        }
                        return aLower > bLower ? 1 : -1;
                    })
                    .map(groupMemberDescriptor => {
                        if (groupMemberDescriptor !== null) {
                            return <tr key={groupMemberDescriptor.id}><td>{groupMemberDescriptor.name ? groupMemberDescriptor.name : <i>Verwijderd</i>}</td><td>{groupMemberDescriptor.present ? "Ja" : "Nee"}</td></tr>;
                        }
                        return null;
                    })}
                </tbody>
            </Table>
        </div>;
    }
}