# Assignment3 part 2

Opgave 1: Automatisk opdatering af total_amount i Orders
I denne opgave løste vi problemet med at holde total_amount i Orders-tabellen opdateret automatisk, når der sker ændringer i OrderDetails.

DELIMITER //
CREATE TRIGGER after_delete_order_detail
AFTER DELETE ON OrderDetails
FOR EACH ROW
BEGIN
  UPDATE Orders
  SET total_amount = (
    SELECT SUM(quantity * price)
    FROM OrderDetails
    WHERE order_id = OLD.order_id
  )
  WHERE order_id = OLD.order_id;
END;

//
DELIMITER ;


DELIMITER //

CREATE TRIGGER after_update_order_detail
AFTER UPDATE ON OrderDetails
FOR EACH ROW
BEGIN
  UPDATE Orders
  SET total_amount = (
    SELECT SUM(quantity * price)
    FROM OrderDetails
    WHERE order_id = NEW.order_id
  )
  WHERE order_id = NEW.order_id;
END;

//
DELIMITER ;


DELIMITER //

CREATE TRIGGER after_insert_order_detail
AFTER INSERT ON OrderDetails
FOR EACH ROW
BEGIN
  UPDATE Orders
  SET total_amount = (
    SELECT SUM(quantity * price)
    FROM OrderDetails
    WHERE order_id = NEW.order_id
  )
  WHERE order_id = NEW.order_id;
END;

//
DELIMITER ;

Fordele:
Hurtigere rapportering og forespørgsler, fordi total_amount altid er forudberegnet.

Ulemper:
Risiko for inkonsistens, hvis triggers ikke fungerer korrekt, eller hvis OrderDetails ændres uden triggers.

Kort forklaring
Vi har oprettet tre triggers:
Når der indsættes, opdateres eller slettes en linje i OrderDetails, genberegnes total_amount i den tilhørende ordre i Orders.
Dette sikrer, at total_amount altid er korrekt uden manuel opdatering.

Opgave 2: 

DELIMITER //

CREATE TRIGGER after_customer_update
AFTER UPDATE ON Customers
FOR EACH ROW
BEGIN
  UPDATE Orders
  SET customer_name = NEW.name,
      customer_email = NEW.email
  WHERE customer_id = NEW.customer_id;
END;

//
DELIMITER ;
Diskussion (forklaring):
Fordele: Mindre behov for joins, hurtigere læsning.
Ulemper: Hvis kundens info ændres, skal man opdatere alle ordrer manuelt.

Opgave 3: 

Diskussion:

Fordele: Hurtigere forespørgsler, fordi MySQL kun søger i én partition.

Udfordringer: Du skal tilføje nye partitioner hvert år manuelt.

MySQL tillader ikke foreign keys i partitioner – fordi partitionering kan gøre det svært at håndhæve referencer korrekt.


Hvordan forbedrer partitionering forespørgselshastigheden?
Partitionering forbedrer hastigheden ved at opdele en stor tabel i mindre dele, så MySQL kun behøver at læse de relevante partitioner i stedet for hele tabellen. Det reducerer mængden af data, der skal behandles, og gør forespørgsler hurtigere.

Hvorfor tillader MySQL ikke fremmednøgler i partitionerede tabeller?
MySQL tillader ikke fremmednøgler i partitionerede tabeller, fordi det ville kræve kontrol på tværs af alle partitioner, hvilket er komplekst og kan påvirke performance og dataintegritet negativt.

Hvad sker der, når et nyt år starter?
Når et nyt år starter, skal man manuelt tilføje en ny partition, ellers vil indsættelse af data for det nye år fejle.

Opgave 4: 

Diskussion:

Fordele: God til data som deles op i faste grupper (fx regioner).

Ved ny region: Du skal ændre strukturen og tilføje en ny partition.

Sammenligning: Range partition bruges til datoer/tal. List partition er bedre til kategorier (som regioner, produkter, statusser).

Liste-partitionering gør forespørgsler på bestemte værdier hurtigere, fordi databasen springer direkte til den rigtige partition.
Hvis en ny region skal tilføjes, skal partitionerne manuelt ændres.
Liste-partitionering bruges ved specifikke værdier, mens rækkevidde-partitionering bruges til intervaller.




