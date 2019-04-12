import React, { Component } from 'react';

export class Manual extends Component {
    render() {
        return <div>
            <h1>Handleiding</h1>
            <h2>Hoe deze app werkt</h2>
            Deze app laat groepen mensen huishoudelijke taken verdelen op basis van een scoresysteem.

            <h2>Registreren</h2>
            Je kunt pas gebruik maken van de app als je een account aanmaakt. Dit kan je doen op de <b>Registreren</b>-pagina. Als je eenmaal ingelogd bent, hoef je dit als het goed is niet nog een keer te doen, tenzij je de app verwijdert.

            <h2>Groepen</h2>
            In een groep kun je mensen toevoegen. Dit kunnen andere gebruikers zijn maar ook gewoon namen van mensen. In een groep kan je taakgroepen aanmaken. Die bestaan uit taken die bij elkaar horen (zoals tafeldekken en afruimen). Je kan een groep aanmaken in het menu.

            <h2>Taakgroepen</h2>
            In een taakgroep kun je dus taken aanmaken. Deze taken kun je een score toekennen. Hiermee geef je aan hoeveel punten iemand kan verdienen met een taak. Je kunt een taak ook score-neutraal maken. Als je één of meerdere score-neutrale taken doet, je score mee omhoog schuift met het gemiddelde. Je krijgt voor een score-neutrale taak dus geen vooraf bepaald aantal punten.

            <h2>Taakverdelingen</h2>
            Per taakgroep kun je een taakverdeling maken. Een taakverdeling geeft aan wie welke taak moet doen. Voordat je een taakverdeling kan aanmaken, moet je eerst aangeven wie er aanwezig is. Iedereen die niet aanwezig is krijgt geen taken en wordt qua score automatisch omhoog geschoven met het gemiddelde. Taakverdelingen kunnen handmatig veranderd worden door op een taak te klikken. Pas als een taakverdeling definitief wordt gemaakt, worden de punten toegekend.
        </div>;
    }
}