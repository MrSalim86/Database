using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace E_Sport
{
    public class EsFunctions
    {
        private readonly string _connectionString;

        public EsFunctions()
        {
            // Læs config fra appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // vigtigt
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void CallJoinTournament(int playerId, int tournamentId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("joinTournament", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@in_player_id", playerId);
            cmd.Parameters.AddWithValue("@in_tournament_id", tournamentId);

            cmd.ExecuteNonQuery();
        }

        public void JoinTournamentDirect(int playerId, int tournamentId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string sql = @"
            INSERT INTO Tournament_Registrations (tournament_id, player_id)
            VALUES (@tournament_id, @player_id);";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tournament_id", tournamentId);
            cmd.Parameters.AddWithValue("@player_id", playerId);

            cmd.ExecuteNonQuery();
        }

        public void CallSubmitMatchResult(int matchId, int winnerId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("submitMatchResult", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@in_match_id", matchId);
            cmd.Parameters.AddWithValue("@in_winner_id", winnerId);

            cmd.ExecuteNonQuery();
        }

        public void SubmitMatchResultDirect(int matchId, int winnerId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string sql = @"
            UPDATE Matches
            SET winner_id = @winner_id
            WHERE match_id = @match_id AND winner_id IS NULL;";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@match_id", matchId);
            cmd.Parameters.AddWithValue("@winner_id", winnerId);

            cmd.ExecuteNonQuery();
        }
    }
}
