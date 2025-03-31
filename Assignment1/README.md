# 📊 SQL-forespørgsler til E-sports Database

Herunder findes en række SQL-forespørgsler til analyse og håndtering af E-sports databasen, der dækker spillere, turneringer, tilmeldinger og kampe.

---

## 1. Hent alle turneringer, der starter inden for de næste 30 dage

```sql
SELECT *
FROM Tournaments
WHERE start_date BETWEEN CURRENT_DATE AND DATE_ADD(CURRENT_DATE, INTERVAL 30 DAY);
```

🔍 Hvad den gør:  
Henter alle turneringer, hvor `start_date` ligger mellem dags dato og 30 dage frem.

---

## 2. Find det antal turneringer, en spiller har deltaget i

```sql
SELECT player_id, COUNT(*) AS tournaments_joined
FROM Tournament_Registrations
GROUP BY player_id;
```

🔍 Hvad den gør:  
Tæller hvor mange turneringer hver spiller er tilmeldt via `Tournament_Registrations`.

---

## 3. Vis en liste over spillere registreret i en bestemt turnering

```sql
SELECT P.*
FROM Players P
JOIN Tournament_Registrations TR ON P.player_id = TR.player_id
WHERE TR.tournament_id = 1;
```

🔍 Hvad den gør:  
Henter alle spillere, der er tilmeldt turnering med ID 1.

---

## 4. Find spillere med flest sejre i en bestemt turnering

```sql
SELECT winner_id, COUNT(*) AS wins
FROM Matches
WHERE tournament_id = 1 AND winner_id IS NOT NULL
GROUP BY winner_id
ORDER BY wins DESC;
```

🔍 Hvad den gør:  
Viser hvilke spillere der har vundet flest kampe i turnering 1.

---

## 5. Hent alle kampe, hvor en bestemt spiller har deltaget

```sql
SELECT *
FROM Matches
WHERE player1_id = 2 OR player2_id = 2;
```

🔍 Hvad den gør:  
Henter alle kampe, hvor spiller med ID 2 deltog – uanset om vedkommende var `player1` eller `player2`.

---

## 6. Hent en spillers tilmeldte turneringer

```sql
SELECT T.*
FROM Tournaments T
JOIN Tournament_Registrations TR ON T.tournament_id = TR.tournament_id
WHERE TR.player_id = 2;
```

🔍 Hvad den gør:  
Viser alle turneringer, hvor spiller med ID 2 er tilmeldt.

---

## 7. Find de 5 bedst rangerede spillere

```sql
SELECT *
FROM Players
ORDER BY ranking DESC
LIMIT 5;
```

🔍 Hvad den gør:  
Henter top 5 spillere med den højeste ranking.

---

## 8. Beregn gennemsnitlig ranking for alle spillere

```sql
SELECT AVG(ranking) AS average_ranking
FROM Players;
```

🔍 Hvad den gør:  
Udregner og viser gennemsnittet af spilleres `ranking`.

---

## 9. Vis turneringer med mindst 5 deltagere

```sql
SELECT T.tournament_id, T.name, COUNT(TR.player_id) AS num_players
FROM Tournaments T
JOIN Tournament_Registrations TR ON T.tournament_id = TR.tournament_id
GROUP BY T.tournament_id, T.name
HAVING COUNT(TR.player_id) >= 5;
```

🔍 Hvad den gør:  
Viser turneringer hvor mindst 5 spillere er registreret.

---

## 10. Find det samlede antal spillere i systemet

```sql
SELECT COUNT(*) AS total_players
FROM Players;
```

🔍 Hvad den gør:  
Tæller det totale antal registrerede spillere i systemet.

---

## 11. Find alle kampe, der mangler en vinder

```sql
SELECT *
FROM Matches
WHERE winner_id IS NULL;
```

🔍 Hvad den gør:  
Henter alle kampe hvor `winner_id` ikke er angivet – altså ingen vinder endnu.

---

## 12. Vis de mest populære spil baseret på turneringsantal

```sql
SELECT game, COUNT(*) AS tournament_count
FROM Tournaments
GROUP BY game
ORDER BY tournament_count DESC;
```

🔍 Hvad den gør:  
Viser hvor mange turneringer der findes pr. spil, og sorterer dem efter antal.

---

## 13. Find de 5 nyeste oprettede turneringer

```sql
SELECT *
FROM Tournaments
ORDER BY created_at DESC
LIMIT 5;
```

🔍 Hvad den gør:  
Henter de 5 senest oprettede turneringer baseret på `created_at`.

---

## 14. Find spillere, der har registreret sig i flere end 3 turneringer

```sql
SELECT player_id, COUNT(*) AS registrations
FROM Tournament_Registrations
GROUP BY player_id
HAVING COUNT(*) > 3;
```

🔍 Hvad den gør:  
Viser spillere, som er tilmeldt mere end 3 turneringer.

---

## 15. Hent alle kampe i en turnering sorteret efter dato

```sql
SELECT *
FROM Matches
WHERE tournament_id = 1
ORDER BY match_date ASC;
```

🔍 Hvad den gør:  
Henter alle kampe i turnering 1 og sorterer dem efter kampdato i stigende rækkefølge.

---

# 📊 Stored procedures 

## 1. registerPlayer
```sql
DELIMITER //

CREATE PROCEDURE registerPlayer (
    IN in_username VARCHAR(50),
    IN in_email VARCHAR(100),
    IN in_ranking INT
)
BEGIN
    INSERT INTO Players (username, email, ranking)
    VALUES (in_username, in_email, in_ranking);
END //

DELIMITER ;
```
🔍 Hvordan bruger man den:
```sql
CALL registerPlayer('PlayerX', 'playerx@example.com', 1234);
```
Beskrivelse:
Proceduren registerPlayer tager 3 input-parametre: username, email og ranking.

Når du kalder CALL registerPlayer(...), oprettes en ny spiller i Players-tabellen med de værdier. 

## 2. joinTournament
```sql
DELIMITER //

CREATE PROCEDURE joinTournament (
    IN in_player_id INT,
    IN in_tournament_id INT
)
BEGIN
    -- Tjek om spilleren allerede er tilmeldt turneringen
    IF NOT EXISTS (
        SELECT 1
        FROM Tournament_Registrations
        WHERE player_id = in_player_id AND tournament_id = in_tournament_id
    ) THEN
        INSERT INTO Tournament_Registrations (tournament_id, player_id)
        VALUES (in_tournament_id, in_player_id);
    END IF;
END //

DELIMITER ;
```
🔍 Hvordan bruger man den:

```sql
CALL joinTournament(2, 1);
```
Beskrivelse:
Tager 2 input-parametre: player_id og tournament_id

Tjekker, om spilleren allerede er tilmeldt turneringen

Tilmeld spilleren, hvis det ikke er tilfældet

## 3. submitMatchResult
```sql
DELIMITER //

CREATE PROCEDURE submitMatchResult (
    IN in_match_id INT,
    IN in_winner_id INT
)
BEGIN
    -- Tjek om kampen eksisterer og har ingen vinder endnu
    IF EXISTS (
        SELECT 1
        FROM Matches
        WHERE match_id = in_match_id AND winner_id IS NULL
    ) THEN
        -- Opdater kampen med vinderen
        UPDATE Matches
        SET winner_id = in_winner_id
        WHERE match_id = in_match_id;
    END IF;
END //

DELIMITER ;
```
🔍 Hvordan bruger man den:
```sql
CALL submitMatchResult(1, 1);
```
Beskrivelse:
Tager to inputparametre: match_id og winner_id

Tjekker om kampen findes og ikke allerede har en vinder

Hvis ja, opdateres winner_id i Matches

# 📊 Functions 

## 1. getTotalWins(player_id)
```sql
DELIMITER //

CREATE FUNCTION getTotalWins(in_player_id INT)
RETURNS INT
DETERMINISTIC
READS SQL DATA
BEGIN
    DECLARE total_wins INT;

    SELECT COUNT(*) INTO total_wins
    FROM Matches
    WHERE winner_id = in_player_id;

    RETURN total_wins;
END //

DELIMITER ;
```
🔍 Hvordan bruger man den:
```sql
SELECT getTotalWins(1) AS wins;
```
Beskrivelse:
Funktionen tager en player_id som input

Den tæller hvor mange gange denne spiller er registreret som vinder i Matches

Returnerer tallet som resultat

## 2. getTournamentStatus(tournament_id)
```sql
DELIMITER //

CREATE FUNCTION getTournamentStatus(in_tournament_id INT)
RETURNS VARCHAR(20)
DETERMINISTIC
READS SQL DATA
BEGIN
    DECLARE status VARCHAR(20);
    DECLARE total_matches INT;
    DECLARE completed_matches INT;
    DECLARE start_date DATE;

    -- Hent startdato
    SELECT T.start_date INTO start_date
    FROM Tournaments T
    WHERE T.tournament_id = in_tournament_id;

    -- Tæl antal kampe i turneringen
    SELECT COUNT(*) INTO total_matches
    FROM Matches
    WHERE tournament_id = in_tournament_id;

    -- Tæl antal kampe med en vinder
    SELECT COUNT(*) INTO completed_matches
    FROM Matches
    WHERE tournament_id = in_tournament_id AND winner_id IS NOT NULL;

    -- Bestem status
    IF CURDATE() < start_date THEN
        SET status = 'upcoming';
    ELSEIF total_matches > 0 AND total_matches = completed_matches THEN
        SET status = 'completed';
    ELSE
        SET status = 'ongoing';
    END IF;

    RETURN status;
END //

DELIMITER ;

```
🔍 Hvordan bruger man den:
```sql
SELECT getTournamentStatus(1) AS status;
```
Beskrivelse:
Tjekker om turneringen endnu ikke er startet → upcoming

Hvis alle kampe i turneringen har en winner_id, så er den completed

Hvis nogle kampe mangler en vinder, men turneringen er startet, er den ongoing

# 📊 Triggers

## 1. beforeInsertRegistration
```sql
DELIMITER //

CREATE TRIGGER beforeInsertRegistration
BEFORE INSERT ON Tournament_Registrations
FOR EACH ROW
BEGIN
    DECLARE current_player_count INT;
    DECLARE max_allowed_players INT;

    -- Tæl hvor mange spillere allerede er tilmeldt denne turnering
    SELECT COUNT(*) INTO current_player_count
    FROM Tournament_Registrations
    WHERE tournament_id = NEW.tournament_id;

    -- Find max tilladte spillere for turneringen
    SELECT max_players INTO max_allowed_players
    FROM Tournaments
    WHERE tournament_id = NEW.tournament_id;

    -- Hvis tilmeldte spillere >= max, så fejl
    IF current_player_count >= max_allowed_players THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Max antal spillere er nået for denne turnering.';
    END IF;
END //

DELIMITER ;
```
Beskrivelse:
Kører før en spiller bliver registreret i Tournament_Registrations

Tæller hvor mange spillere der allerede er registreret til den turnering

Sammenligner med max_players fra Tournaments

Hvis grænsen er nået, stopper den med en fejlbesked

## 2. afterInsertMatch
```sql
DELIMITER //

CREATE TRIGGER afterInsertMatch
AFTER INSERT ON Matches
FOR EACH ROW
BEGIN
    DECLARE loser_id INT;

    -- Tjek at der er en vinder
    IF NEW.winner_id IS NOT NULL THEN
        -- Find taberens ID
        IF NEW.player1_id = NEW.winner_id THEN
            SET loser_id = NEW.player2_id;
        ELSE
            SET loser_id = NEW.player1_id;
        END IF;

        -- Opdater ranking
        UPDATE Players SET ranking = ranking + 10 WHERE player_id = NEW.winner_id;
        UPDATE Players SET ranking = ranking - 5 WHERE player_id = loser_id;
    END IF;
END //

DELIMITER ;
```
Beskrivelse:
Kører automatisk efter en ny kamp bliver tilføjet til Matches

Finder taberen ved at sammenligne player1_id, player2_id og winner_id

Opdaterer ranking: +10 til vinder, -5 til taber

Hvis winner_id IS NULL, så sker intet
