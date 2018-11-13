import React, { Component } from 'react';
import { ListGroup, ListGroupItem, Glyphicon } from 'react-bootstrap';

export class ExpandableListGroup extends Component {
    constructor(props) {
        super(props);
        this.state = {
            visibleItems: this.props.visibleItems
        };
    }

    showMoreTaskGroupRecords = () => {
        this.setState({ visibleItems: this.state.visibleItems + 5 });
    };

    flattenList = list => {
        let output = [];
        list.forEach(subList => output = output.concat(subList));
        return output;
    }

    render() {
        return <ListGroup>
            {this.flattenList(this.props.children).slice(0, this.state.visibleItems)}
            {this.flattenList(this.props.children).length > this.state.visibleItems ? 
                <ListGroupItem onClick={this.showMoreTaskGroupRecords}>
                    <Glyphicon glyph="chevron-down" /> <i>Laat meer zien&#8230;</i>
                </ListGroupItem>
            : null}
        </ListGroup>
    }
}

ExpandableListGroup.defaultProps = {
    visibleItems: 5
};