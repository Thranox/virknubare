## Udkast til API
_Version: 2020-06-10_

API'en tager udgangspunkt i i følgende:
* Felter nævnt på: https://improvento.atlassian.net/wiki/spaces/POL/pages/731217935/Felter+p+afregningsformularen
* Felter i nuværende løsning: https://www.politikerafregning.dk/DEM/Rejseafregning.nsf/Main.html

POST og GET returnere/modtager samme objekt.

### Todo
- Afklar om felternes engelske oversættelser er OK.
- Bekræft om alle ønskede felter er i API'en.
- Bekræft om


## Rejseafregning
GET /travelexpenses/1
```json5
{
  // Møde- og rejseoplysninger

  departureDateTime: "2018-05-28T15:01:01.111Z", // Udrejsedato/tidspunkt
  arrivalDateTime: "2018-05-30T15:01:01.111Z", // Hjemkomstdato/tidspunkt

  committee: "Lokaludvalg for bedre veje", // Udvalg
  purpose: "Opfølgning", // Mødets formål

  destinationPlace: "Skanderborgvej 33, 8680 Ry", // Mødested
  departurePlace: "Åvej 32, 8000 Aarhus", // Udrejsested
  arrivalPlace: "Åvej 32, 8000 Aarhus", // Hjemkomststed

  // Afregningsoplysninger

  isEducationalPurpose: true, // Kursus/Seminar
  transportSpecification: {
    method: "car", // car,bike
    kilometers: 88,
    numberPlate: "AA12345"
  },
  isAbsenceAllowance: true, // Fraværsgodtgørelse
  dailyAllowance: { // Diæter.
    lessThan4hours: 2,
    moreThan4hours: 0,
  },
  lossOfEarnings: [ // Specificering af tabt arbejdsfortjeneste (hentes fra et endpoint ud fra user/company)
    {id:  12, amount:  3}, // ex. sats 300
    {id:  13, amount:  1}, // ex. sats 400
    {id:  15, amount:  0}  // ex. sats 500
  ],

  // Processstyring
  flowStep:  {
    id: 444,
    description: "Anvist til betaling",
  },
  allowedFlowSteps: [
    {flowStepId: 424, description: 'Udbetal'}
  ],
}
```

## Process (flowstep)

POST /travelexpenses/1/flowstep/{flowStepId}
```json5
// tom body
```

## Bilag

#### Hent list af bilag
GET /travelexpenses/1/attachments
```json5
[
  {
    id: 14,
    fileName: "kvittering.jpg",
    fileSize: 3252626, // in bytes
    mimeType: "image/jpeg", // in bytes,
    url: "https://.....",
  },
  // ...
]
```

#### Opret nyt bilag
POST /travelexpenses/1/attachements
```json5
{
    fileName: "kvittering.jpg",
    fileSize: 3252626, // in bytes
    mimeType: "image/jpeg", // in bytes,
    date: ".....", // Det kan også sendes som normal multipart/formdata
    url: "https://.....",
}
```

#### Slet bilag
DELETE /travelexpenses/1/attachements
```json5
// tom body
```



