import React, { Component } from 'react';
import { ListGroupItem, Glyphicon } from 'react-bootstrap';

export class ExpandableCollection extends Component {
    constructor(props) {
        super(props);
        this.state = {
            visibleItems: this.props.visibleItems
        };
    }

    showMoreItems = () => {
        this.setState({ visibleItems: this.state.visibleItems + 20 });
    };

    flattenList = list => {
        let output = [];
        list.forEach(subList => output = output.concat(subList));
        return output;
    }

    render() {
        const flattenedList = this.flattenList(this.props.children);
        return [
            flattenedList.slice(0, flattenedList.length === this.state.visibleItems + 1 ? this.state.visibleItems + 1 : this.state.visibleItems),
            flattenedList.length > this.state.visibleItems + 1 ? 
                <ListGroupItem onClick={this.showMoreItems} key="show-more">
                    <Glyphicon glyph="chevron-down" /> <i>Laat meer zien&#8230;</i>
                </ListGroupItem>
            : null
        ];
    }
}

ExpandableCollection.defaultProps = {
    visibleItems: 5
};