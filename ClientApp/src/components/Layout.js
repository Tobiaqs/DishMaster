import React, { Component } from 'react';
import { Col, Container, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  render() {
    return <span>
      <NavMenu />
      <Container fluid>
      <Row>
        <Col>
          {this.props.children}
        </Col>
      </Row>
    </Container>
  </span>;
  }
}
