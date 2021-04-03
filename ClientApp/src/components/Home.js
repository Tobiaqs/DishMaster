import React, { Component } from 'react';

export class Home extends Component {
  render() {
    return (
      <div>
        <h1>DishMaster</h1>
        <p>Dit is <b>DishMaster</b>. Dé app om je te helpen te besluiten wie de afwas (en andere taken) moet doen op basis van een scoresysteem!</p>
        <ul>
          <li>Eerlijk omdat iedereen automatisch naar een gemiddelde score streeft;</li>
          <li>Er wordt rekening gehouden met mensen die er niet zijn;</li>
          <li>Handige interface.</li>
        </ul>
      </div>
    );
  }
}
