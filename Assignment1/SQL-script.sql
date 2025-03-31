-- Opret schema
CREATE DATABASE IF NOT EXISTS E_sports;
USE E_sports;

-- 1. Players
CREATE TABLE Players (
    player_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    ranking INT NOT NULL DEFAULT 1000,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 2. Tournaments
CREATE TABLE Tournaments (
    tournament_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    game VARCHAR(50) NOT NULL,
    max_players INT NOT NULL CHECK (max_players > 0),
    start_date DATE NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 3. Tournament_Registrations
CREATE TABLE Tournament_Registrations (
    registration_id INT AUTO_INCREMENT PRIMARY KEY,
    tournament_id INT NOT NULL,
    player_id INT NOT NULL,
    registered_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (tournament_id) REFERENCES Tournaments(tournament_id) ON DELETE CASCADE,
    FOREIGN KEY (player_id) REFERENCES Players(player_id) ON DELETE CASCADE,
    UNIQUE (tournament_id, player_id)
);

-- 4. Matches
CREATE TABLE Matches (
    match_id INT AUTO_INCREMENT PRIMARY KEY,
    tournament_id INT NOT NULL,
    player1_id INT NOT NULL,
    player2_id INT NOT NULL,
    winner_id INT,
    match_date DATETIME NOT NULL,
    FOREIGN KEY (tournament_id) REFERENCES Tournaments(tournament_id) ON DELETE CASCADE,
    FOREIGN KEY (player1_id) REFERENCES Players(player_id),
    FOREIGN KEY (player2_id) REFERENCES Players(player_id),
    FOREIGN KEY (winner_id) REFERENCES Players(player_id),
    CHECK (player1_id <> player2_id)
);

-- Dummy data: Players
INSERT INTO Players (username, email, ranking)
VALUES 
('GamerOne', 'gamer1@example.com', 1200),
('ShadowX', 'shadowx@example.com', 1300),
('Nova99', 'nova99@example.com', 1100),
('Blade', 'blade@example.com', 1400);

-- Dummy data: Tournaments
INSERT INTO Tournaments (name, game, max_players, start_date)
VALUES 
('Spring Showdown', 'Valorant', 16, '2025-04-15'),
('Summer Smash', 'League of Legends', 8, '2025-06-01');

-- Dummy data: Tournament_Registrations
INSERT INTO Tournament_Registrations (tournament_id, player_id)
VALUES 
(1, 1),
(1, 2),
(1, 3),
(2, 2),
(2, 4);

-- Dummy data: Matches
INSERT INTO Matches (tournament_id, player1_id, player2_id, winner_id, match_date)
VALUES 
(1, 1, 2, 1, '2025-04-15 14:00:00'),
(1, 3, 1, 3, '2025-04-16 15:00:00'),
(2, 2, 4, 4, '2025-06-01 12:00:00');
