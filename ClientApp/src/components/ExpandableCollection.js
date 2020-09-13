import React, { Component } from 'react';
import { ListGroupItem } from 'react-bootstrap';
import { FaArrowDown } from 'react-icons/fa';

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
                <ListGroupItem action onClick={this.showMoreItems} key="show-more">
                    <FaArrowDown /> <i>Laat meer zien&#8230;</i>
                </ListGroupItem>
            : null
        ];
    }
}

ExpandableCollection.defaultProps = {
    visibleItems: 5
};