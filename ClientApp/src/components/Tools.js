import React from 'react';

export class Tools {
    static getGroupMemberName(group, groupMemberId) {
        const groupMember = group.groupMembers.find(groupMember => groupMember.id === groupMemberId);
        return Tools.getGroupMemberNameDirect(groupMember);
    }

    static getGroupMemberNameDirect(groupMember) {
        if (groupMember == null) {
            return <i>Niemand</i>;
        }
        return groupMember.isAnonymous ? groupMember.anonymousName : groupMember.fullName;
    }

    static roundScore(score) {
        return Math.round(score * 10) / 10;
    }
}