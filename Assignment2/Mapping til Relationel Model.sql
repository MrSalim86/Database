Table Medlem {
  MedlemsID int [pk, increment]
  Navn varchar
  Email varchar
  Telefonnummer varchar
  Adresse varchar
  Fødselsdato date
  MedlemstypeID int [fk, > Medlemstype.MedlemstypeID]
  Aktiv bool
}

Table Medlemstype {
  MedlemstypeID int [pk, increment]
  Navn varchar
  Beskrivelse text
  PrisPerMåned decimal
  AdgangFaciliteter text
}

Table Træningshold {
  HoldID int [pk, increment]
  Navn varchar
  Beskrivelse text
  MaxDeltagere int
  Starttid datetime
  Sluttid datetime
  Ugedag varchar
  Lokale varchar
}

Table Instruktør {
  InstruktørID int [pk, increment]
  Navn varchar
  Email varchar
  Telefon varchar
  Speciale varchar
}

Table Booking {
  BookingID int [pk, increment]
  MedlemsID int [fk, > Medlem.MedlemsID]
  HoldID int [ref: > Træningshold.HoldID]
  BookingDato datetime
  Status varchar // fx: Booket, Aflyst, Venteliste
}

Table InstruktørHold {
  InstruktørID int [fk, > Instruktør.InstruktørID]
  HoldID int [ref: > Træningshold.HoldID]
  PrimærInstruktør bool
}

Table Betaling {
  BetalingID int [pk, increment]
  MedlemsID int [fk, > Medlem.MedlemsID]
  Betalingsdato date
  Beløb decimal
  Betalingstype varchar // Abonnement / Klippekort
  AntalKlip int
}

Table Rabat {
  RabatID int [pk, increment]
  Navn varchar
  Beskrivelse text
  Procent decimal
  GyldigFra date
  GyldigTil date
}

Table MedlemsRabat {
  MedlemsID int [fk, > Medlem.MedlemsID]
  RabatID int [fk, > Rabat.RabatID]
}
