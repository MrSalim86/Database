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
  
## 2. joinTournament

## 3. submitMatchResult

# 📊 Functions 

## 1. getTotalWins(player_id)

## 2. getTournamentStatus(tournament_id)

# 📊 Triggers

## 1. beforeInsertRegistration

## 2. afterInsertMatch

