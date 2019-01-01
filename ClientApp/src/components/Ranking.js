import React, { Component } from 'react';
import { Bar } from 'react-chartjs';
import { Api } from '../Api';
import { Tools } from './Tools';

export class Ranking extends Component {
    constructor(props) {
        super(props);

        this.state = { group: null };
    }

    componentDidMount() {
        this.fetch();
    }

    componentDidUpdate = (prevProps, prevState, snapshot) => {
        if (prevProps.match.params.groupId !== this.props.match.params.groupId) {
            this.reload();
        }
    }

    fetch = () => {
        Api.getInstance().Group.Get({ groupId: this.props.match.params.groupId }).then(result => {
            this.setState({ group: result.payload });
        });
    }

    reload = () => {
        this.setState({ group: null });
        this.fetch();
    }

    getChartData = () => {
        const sortedGroupMembers = this.state.group.groupMembers.sort((a, b) => {
            if (a.score === b.score) {
                return 0;
            }
            return a.score > b.score ? -1 : 1;
        });

        return {
            labels: sortedGroupMembers.map(Tools.getGroupMemberNameDirect),
            datasets: [{
                data: sortedGroupMembers.map(gm => gm.score)
            }]
        };
    }

    chartOptions = {
        scaleBeginAtZero: false,
        responsive: true
    }

    render() {
        return this.state.group ? <div>
            <h1>Scoreoverzicht</h1>
            <h4>Van hoog naar laag</h4>
            <Bar data={this.getChartData()} options={this.chartOptions} />
        </div> : <h1>Laden&#8230;</h1>;
    }
}