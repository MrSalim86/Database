using MySql.Data.MySqlClient;

namespace Assignment3_Part1
{
    public class TournamentService
    {
        public async Task<bool> UpdateTournamentOptimisticAsync(int tournamentId, DateTime newStartDate)
        {
            using var conn = DbHelper.GetConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var selectCmd = new MySqlCommand("SELECT version FROM Tournaments WHERE tournament_id = @id", conn, (MySqlTransaction)transaction);
                selectCmd.Parameters.AddWithValue("@id", tournamentId);
                var currentVersion = Convert.ToInt32(await selectCmd.ExecuteScalarAsync());

                var updateCmd = new MySqlCommand(
                    "UPDATE Tournaments SET start_date = @startDate, version = version + 1 WHERE tournament_id = @id AND version = @version",
                    conn, (MySqlTransaction)transaction);

                updateCmd.Parameters.AddWithValue("@startDate", newStartDate);
                updateCmd.Parameters.AddWithValue("@id", tournamentId);
                updateCmd.Parameters.AddWithValue("@version", currentVersion);

                if (await updateCmd.ExecuteNonQueryAsync() == 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateMatchPessimisticAsync(int matchId, int winnerId)
        {
            using var conn = DbHelper.GetConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var lockCmd = new MySqlCommand("SELECT * FROM Matches WHERE match_id = @id FOR UPDATE", conn, (MySqlTransaction)transaction);
                lockCmd.Parameters.AddWithValue("@id", matchId);
                await lockCmd.ExecuteReaderAsync().ContinueWith(t => t.Result.Close());

                var updateCmd = new MySqlCommand("UPDATE Matches SET winner_id = @winner_id WHERE match_id = @id", conn, (MySqlTransaction)transaction);
                updateCmd.Parameters.AddWithValue("@winner_id", winnerId);
                updateCmd.Parameters.AddWithValue("@id", matchId);
                await updateCmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> RegisterPlayerAsync(int tournamentId, int playerId)
        {
            using var conn = DbHelper.GetConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var countCmd = new MySqlCommand("SELECT COUNT(*) FROM Tournament_Registrations WHERE tournament_id = @id", conn, (MySqlTransaction)transaction);
                countCmd.Parameters.AddWithValue("@id", tournamentId);
                int currentCount = Convert.ToInt32(await countCmd.ExecuteScalarAsync());

                if (currentCount >= 100)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                var regCmd = new MySqlCommand("INSERT INTO Tournament_Registrations (tournament_id, player_id) VALUES (@tournamentId, @playerId)", conn, (MySqlTransaction)transaction);
                regCmd.Parameters.AddWithValue("@tournamentId", tournamentId);
                regCmd.Parameters.AddWithValue("@playerId", playerId);
                await regCmd.ExecuteNonQueryAsync();

                var updateCmd = new MySqlCommand("UPDATE Players SET ranking = ranking + 5 WHERE player_id = @playerId", conn, (MySqlTransaction)transaction);
                updateCmd.Parameters.AddWithValue("@playerId", playerId);
                await updateCmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdatePlayerRankingAsync(int playerId)
        {
            using var conn = DbHelper.GetConnection();
            await conn.OpenAsync();

            var cmd = new MySqlCommand("CALL UpdateRanking(@playerId)", conn);
            cmd.Parameters.AddWithValue("@playerId", playerId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
